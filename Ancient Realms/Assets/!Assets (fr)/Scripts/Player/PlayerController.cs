using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using ESDatabase.Classes;
using ESDatabase.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;
    [Header("Player Settings")]
    [SerializeField] public Transform attackPoint;
    [SerializeField] public JavelinPrefab javelinPrefab;
    [SerializeField] public Transform javelinPoint;
    [SerializeField] public CinemachineVirtualCamera virtualCamera;
    [SerializeField] public GameObject cm;
    [SerializeField] public Slider forceSlider;
    [SerializeField] public GameObject forceGO;
    [Header("Weapons Unsheath")]
    [SerializeField] public SpriteRenderer mainWeapon;
    [SerializeField] public SpriteRenderer javelin;
    [SerializeField] public SpriteRenderer shieldFront;
    [SerializeField] public SpriteRenderer shieldBack;
    [Header("Weapons Sheath")]
    [SerializeField] public SpriteRenderer mainWeaponBelt;
    [SerializeField] public SpriteRenderer shieldBackStrap;
    [SerializeField] public SpriteRenderer pilumAttach;
    private CinemachineFramingTransposer framingTransposer;
    public LayerMask enemyLayer;
    private Vector2 lastPosition;
    private float distanceMoved;
    private const float moveThreshold = 2f;
    Vector2 moveInput;
    [Header("Player Actions")]
    public bool moveInputActive = false;
    private bool interactPressed = false;
    private bool submitPressed = false;
    public bool isAttacking = false;
    public bool canWalk;
    public bool isBlocking;
    public bool isEquipping = false;
    public float holdTime = 0f;
    public bool isHolding = false;
    public bool isThrowing = false;
    [Header("Restriction")]
    public bool canAccessInventory = true;
    public bool canAccessJournal = true;
    public bool canAccessMap = true;
    public bool canAccessCombatMode = true;
    Rigidbody2D rb;
    public Animator animator;
    private static PlayerController Instance;
    public float panSpeed = 2f;
    private float panDistance = 7f;
    public InputActionAsset inputActions;
    public InputActionMap playerActionMap;
    public InputActionMap chapterSelectActionMap;
    public InputActionMap questActionMap;
    public InputActionMap inventoryActionMap;
    public InputActionMap pauseActionMap;
    public InputActionMap dialogueActionMap;
    public InputActionMap promptActionMap;
    public InputActionMap mapActionMap;
    public InputActionMap mintingActionMap;
    public InputActionMap shopActionMap;
    private Vector3 originalCameraOffset;
    public List<EquipSO> equipmentList;
    public bool isInterior = false;
    private void Awake()
    {
        if(Instance != null){
            Debug.LogWarning("Found more than one Player Controller in the scene");
        }
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        equipmentList = Resources.LoadAll<EquipSO>("EquipSO").ToList();
        LoadPlayerData(PlayerStats.GetInstance());
        if(PlayerStats.GetInstance().firstLogin){
            Vector3 currentPosition = gameObject.transform.position;
            PlayerData playerData = AccountManager.Instance.playerData;
            gameObject.transform.position = new Vector3(playerData.gameData.lastX, playerData.gameData.lastY, currentPosition.z);
            PlayerStats.GetInstance().firstLogin = false;
        }
        playerActionMap = inputActions.FindActionMap("Player");
        chapterSelectActionMap = inputActions.FindActionMap("ChapterSelect");
        questActionMap = inputActions.FindActionMap("Quest");
        inventoryActionMap = inputActions.FindActionMap("Inventory");
        pauseActionMap = inputActions.FindActionMap("Pause");
        dialogueActionMap = inputActions.FindActionMap("Dialogue");
        promptActionMap = inputActions.FindActionMap("Prompt");
        mapActionMap = inputActions.FindActionMap("Map");
        mintingActionMap = inputActions.FindActionMap("Minting");
        shopActionMap = inputActions.FindActionMap("Shop");
        playerActionMap.Disable();
    }
    
    private void Start(){
        playerStats = PlayerStats.GetInstance();
        lastPosition = transform.position;
        originalCameraOffset = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
    }
    public static PlayerController GetInstance(){
        return Instance;
    }
    private void OnEnable(){
        LocationSO locationSettings = LocationSettingsManager.GetInstance().locationSettings;
        canAccessCombatMode = locationSettings.canAccessCombatMode;
        canAccessInventory = locationSettings.canAccessInventory;
        canAccessMap = locationSettings.canAccessMap;
        canAccessJournal = locationSettings.canAccessJournal;
        isInterior = PlayerStats.GetInstance().localPlayerData.gameData.isInterior;
        PlayerStats.GetInstance().toggleStamina = locationSettings.toggleStamina;
    }
    public bool GetInteractPressed() 
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public async void FixedUpdate()
    {
        if(PlayerStats.GetInstance().currentHP <= 0 && !PlayerStats.GetInstance().isDead){
            PlayerStats.GetInstance().isDead = true;
            PlayerStats.GetInstance().AddStatistics(StatisticsType.DeathTotal, "1");
            animator.Play("Death");
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            playerActionMap.Disable();
            await PlayerUIManager.GetInstance().ClosePlayerUI();
            await PlayerUIManager.GetInstance().OpenDeathUI();
            return;
        }
        if(!PlayerStats.GetInstance().isDead){
            if (!moveInputActive && IsRunning || !IsRunning && canWalk)
            {
                if (playerStats.isCombatMode)
                {
                    playerStats.walkSpeed = Mathf.Max(playerStats.walkSpeed, playerStats.walkSpeed - (playerStats.walkSpeed * 0.25f) * Time.deltaTime);
                    playerStats.staminaRegenRate = Mathf.Max(playerStats.staminaRegenRate, playerStats.staminaRegenRate - (playerStats.staminaRegenRate * 0.25f) * Time.deltaTime);
                }

                // Stamina management logic
                if (playerStats.isCombatMode && IsMoving && isBlocking)
                {
                    playerStats.stamina = Mathf.Max(0, playerStats.stamina - (playerStats.staminaDepletionRate * 0.5f) * Time.deltaTime);
                }
                else if (playerStats.isCombatMode && !IsMoving && isBlocking)
                {
                    // Standing and blocking - regenerate stamina
                    playerStats.stamina = Mathf.Min(playerStats.maxStamina, playerStats.stamina + (playerStats.staminaRegenRate * 0.25f) * Time.deltaTime);
                }
                else if (playerStats.isCombatMode && IsMoving)
                {
                    playerStats.stamina = Mathf.Max(0, playerStats.stamina - (playerStats.staminaDepletionRate * 0.25f) * Time.deltaTime);
                }
                else
                {
                    // Regular regeneration when not moving
                    playerStats.stamina = Mathf.Min(playerStats.maxStamina, playerStats.stamina + playerStats.staminaRegenRate * Time.deltaTime);
                }
            }
            if (DialogueManager.GetInstance().dialogueIsPlaying)
            {
                // Stop player movement during dialogue or interaction
                rb.velocity = Vector2.zero;
                IsRunning = false;
                IsMoving = false;
                if(playerStats.isCombatMode){
                    animator.SetBool("isCombatMode", false);
                    playerStats.isCombatMode = false;
                    isEquipping = true;
                }
                playerActionMap.Disable();
                dialogueActionMap.Enable();
                return;
            }
            if (IsMoving)
            {
                if (IsRunning && moveInputActive)
                {
                    if (playerStats.stamina > 0 && playerStats.toggleStamina)
                    {
                        playerStats.stamina = Mathf.Max(0, playerStats.stamina - playerStats.staminaDepletionRate * Time.deltaTime);
                        if (playerStats.stamina < 1)
                        {
                            IsRunning = false; // Stop running if stamina is too low
                        }
                    }
                }
            }
            if(canWalk && !isEquipping){
                if (moveInputActive)
                {
                    SetFacingDirection(moveInput);
                    if(isHolding) return;
                    IsMoving = true;
                    rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
                    Vector2 currentPosition = transform.position;
                    float deltaX = currentPosition.x - lastPosition.x;
                    
                    if (Mathf.Abs(deltaX) > 0.01f)
                    {
                        distanceMoved += Mathf.Abs(deltaX);
                        if (distanceMoved >= moveThreshold)
                        {
                            PlayerStats.GetInstance().AddStatistics(StatisticsType.MoveDistance, "1");
                            if (IsRunning == true){
                                List<QuestSO> quest = playerStats.activeQuests.ToList();
                                foreach(QuestSO q in quest){
                                    if(q.goals[q.currentGoal].goalType == GoalTypeEnum.RunLeft || q.goals[q.currentGoal].goalType == GoalTypeEnum.RunRight){
                                        QuestManager.GetInstance().UpdateRunGoals(deltaX);
                                    } 
                                }
                            }else if (!IsRunning){
                                List<QuestSO> quest = playerStats.activeQuests.ToList();
                                foreach(QuestSO q in quest){
                                    if(q.goals[q.currentGoal].goalType == GoalTypeEnum.WalkLeft || q.goals[q.currentGoal].goalType == GoalTypeEnum.WalkRight){
                                        QuestManager.GetInstance().UpdateWalkGoals(deltaX);
                                    } 
                                }
                                
                            }
                            distanceMoved = 0f;
                            // Reset the distanceMoved counter
                        }
                        lastPosition = currentPosition;
                    }

                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
            //<-- Equipping -->
            LoadPlayerData(PlayerStats.GetInstance());
            // <-- Movement Tracking -->
            float x = PlayerStats.GetInstance().localPlayerData.gameData.lastX;
            float y = PlayerStats.GetInstance().localPlayerData.gameData.lastY;
            bool isInter = PlayerStats.GetInstance().localPlayerData.gameData.isInterior;
        
            if (transform.position.x != x || transform.position.y != y || isInter != isInterior)
            {
                PlayerStats.GetInstance().localPlayerData.gameData.lastX = transform.position.x;
                PlayerStats.GetInstance().localPlayerData.gameData.lastY = transform.position.y;
                PlayerStats.GetInstance().localPlayerData.gameData.isInterior = isInterior;
                PlayerStats.GetInstance().isDataDirty = true;
            }
        }
    }

    private void LoadPlayerData(PlayerStats player){
        List<EquipmentSO> equippedItems = player.equippedItems;

        EquipmentSO main = PlayerStats.GetInstance().equippedItems[4];
        EquipmentSO shield = PlayerStats.GetInstance().equippedItems[5];
        EquipmentSO jav = PlayerStats.GetInstance().equippedItems[6];
        if(main != null){
            mainWeapon.sprite = equippedItems[4].front;
            mainWeaponBelt.sprite = equippedItems[4].front;
        }else{
            EquipmentSO pugio = AccountManager.Instance.equipments.FirstOrDefault(equip => equip.equipmentId == 13);
            mainWeapon.sprite = pugio.front;
            mainWeaponBelt.sprite = pugio.front;
        }
        if(shield != null){
            shieldFront.sprite = equippedItems[5].front;
            shieldBack.sprite = equippedItems[5].back;
        }else{
            shieldFront.sprite = null;
            shieldBack.sprite = null;
        }
        if(jav != null){
            javelin.sprite = equippedItems[6].front;
        }else{
            javelin.sprite = null;
        }
        
    }
    public void Attack(InputAction.CallbackContext context){
        if(context.performed){
            if(playerStats.stamina < 10 || isHolding || isEquipping || isAttacking || isThrowing) return;
            if(PlayerStats.GetInstance().isCombatMode && !IsRunning && !isHolding){
                isAttacking = true;
                playerStats.stamina -= 10f;
            }else{
                return;
            }
        }
    }
    public void OnThrowPilum(InputAction.CallbackContext context)
    {
        EquipmentSO pilum = PlayerStats.GetInstance().equippedItems[6];
        if (context.started)
        {
            if(playerStats.stamina < 10) return;
            if(pilum == null){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "You don't have a javelin");
                return;
            }
            if(isAttacking) return;
            if(playerStats.isCombatMode && !isAttacking && !IsRunning){
                isHolding = true;
                isThrowing = true;
                animator.SetBool("isHolding", true);
            }
        }else if (context.canceled)
        {
            // Release the W key
            isHolding = false;
            animator.SetBool("isHolding", false);
        }

    }
    private void ThrowPilum()
    {
        float maxDamage = playerStats.equippedItems[6].baseDamage * 1.5f;

        // Calculate the throw force and corresponding damage based on hold time
        float damage = Mathf.Lerp(0, maxDamage, holdTime / playerStats.maxHoldTime);

        // Instantiate the pilum
        JavelinPrefab pilum = Instantiate(javelinPrefab, javelinPoint.position, javelinPoint.rotation);
        playerStats.stamina -= 10f;

        // Apply force to the pilum in an arching trajectory
        Rigidbody2D rb = pilum.GetComponent<Rigidbody2D>();
        float throwForce = (holdTime / playerStats.maxHoldTime) * playerStats.maxThrowForce;

        float throwAngle = 22f; // Lower angle for a flatter trajectory

        // Adjust the force direction based on whether the player is facing right or left
        Vector2 forceDirection = IsFacingRight ? Vector2.right : Vector2.left;

        Vector2 force = new Vector2(
            forceDirection.x * throwForce * Mathf.Cos(throwAngle * Mathf.Deg2Rad), 
            throwForce * Mathf.Sin(throwAngle * Mathf.Deg2Rad) * 0.5f // Reduce the vertical component
        );

        pilum.SetDamage(damage);

        // Apply the force to the pilum
        rb.AddForce(force, ForceMode2D.Impulse);

        // Apply rotation to the pilum for a realistic spinning effect
        float torqueDirection = IsFacingRight ? -1 : 1; // Reverse torque direction based on facing
        float maxTorque = 0.3f;
        float torqueAmount = Mathf.Lerp(0, maxTorque, throwForce / playerStats.maxThrowForce);

        rb.AddTorque(torqueDirection * torqueAmount, ForceMode2D.Impulse); // Use a fixed torque value

        // Flip the pilum if throwing to the left
        if (!IsFacingRight)
        {
            Vector3 pilumScale = pilum.transform.localScale;
            pilumScale.x *= -1; // Flip the pilum horizontally
            pilum.transform.localScale = pilumScale;
        }

        holdTime = 0f;
        isHolding = false;
    }


    public void PanCameraBasedOnPlayerDirection()
    {
        // Get the current offset of the camera
        Vector3 currentOffset = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;

        // Determine the target offset based on the player's facing direction
        float targetOffsetX = IsFacingRight ? panDistance : -panDistance;

        // Smoothly move the camera to the target offset
        Vector3 targetOffset = new Vector3(targetOffsetX, currentOffset.y, currentOffset.z);
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = Vector3.Lerp(currentOffset, targetOffset, panSpeed * Time.deltaTime);
    }

    public void ResetCameraPan()
    {
        // Reset the camera offset to the original position
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = originalCameraOffset;
    }
    void Applydamage(){
        List<Enemy> enemiesInRange = new List<Enemy>();
        Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerStats.attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies){
            enemiesInRange.Add(enemy.GetComponent<Enemy>());
        }
        Enemy targetEnemy = enemiesInRange.OrderByDescending(enemy => enemy.currentHP).FirstOrDefault();
        if(targetEnemy != null){
            targetEnemy.TakeDamage(playerStats.damage);
        }
        
    }
    void ApplyBash(){
        List<Enemy> enemiesInRange = new List<Enemy>();
        Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerStats.attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies){
            enemiesInRange.Add(enemy.GetComponent<Enemy>());
        }
        Enemy targetEnemy = enemiesInRange.OrderByDescending(enemy => enemy.currentHP).FirstOrDefault();
        if (targetEnemy != null) {
            targetEnemy.MoveBackStun();
        }

    }
    void OnDrawGizmosSelected(){
        if(attackPoint == null) return;
        Gizmos .DrawWireSphere(attackPoint.position, playerStats.attackRange);
    }
    public void Block(InputAction.CallbackContext context){
        EquipmentSO shield = PlayerStats.GetInstance().equippedItems[5];
        if(context.performed){
            if(shield == null){
                PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "You don't have a shield");
                return;
            }
            if(PlayerStats.GetInstance().isCombatMode && IsRunning == false){
                animator.SetBool("isBlocking", true);
                playerStats.stamina -= 5f * Time.deltaTime;
            }
        }else if (context.canceled)
        {
            animator.SetBool("isBlocking", false);
        }
    }
    public void TakeDamage(float damage){
            float newDamage = Utilities.CalculateDamage(damage,  playerStats.armor);
            if(isBlocking){
                playerStats.currentHP -= Utilities.CalculateDamage(damage,  playerStats.armor, true);
            }else{
                playerStats.currentHP -= newDamage;
            }
    }
    void MoveFront()
    {
        // Get the character's current position
        Vector2 cP = gameObject.transform.position;

        // Define the movement distance (adjust as needed)
        float moveDistance = 0.3f;

        // Calculate the new position based on the direction the character is facing
        Vector2 moveDirection = IsFacingRight ? Vector2.right : Vector2.left;

        Vector2 newPosition = cP + moveDirection * moveDistance;

        // Define the duration for the smooth movement (adjust as needed)
        float moveDuration = 0.2f;

        // Use DOTween to smoothly move the character to the new position
        gameObject.transform.DOMove(newPosition, moveDuration).SetEase(Ease.OutQuad).OnKill(() => {
            if (gameObject == null) return;  // Stops movement if the object is destroyed
        });;
        
    }

    public void MoveBack()
    {
        // Get the character's current position
        Vector2 cP = gameObject.transform.position;

        // Define the movement distance (adjust as needed)
        float moveDistance = -0.3f;

        // Calculate the new position based on the direction the character is facing
        Vector2 moveDirection = IsFacingRight ? Vector2.right : Vector2.left;

        Vector2 newPosition = cP + moveDirection * moveDistance;

        // Define the duration for the smooth movement (adjust as needed)
        float moveDuration = 0.2f;

        // Use DOTween to smoothly move the character to the new position
        gameObject.transform.DOMove(newPosition, moveDuration).SetEase(Ease.OutQuad).OnKill(() => {
            if (gameObject == null) return;  // Prevents further movement if destroyed
        });
        
    }
    public void OnMove(InputAction.CallbackContext context) 
    {
            moveInput = context.ReadValue<Vector2>();
            moveInputActive = context.phase == InputActionPhase.Performed;
            
            if (!DialogueManager.GetInstance().dialogueIsPlaying && !interactPressed)
            {
                IsMoving = moveInputActive;
                SetFacingDirection(moveInput);
            }
    }
    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
            // IsMoving = false;
            // IsRunning = false;
            // isAttacking = false;
            // isBlocking = false;
            // isHolding = false;
        }
        else if (context.canceled)
        {
            interactPressed = false;
            // Resume movement if moveInput is still active
            // if (moveInputActive)
            // {
            //     IsMoving = true;
            //     SetFacingDirection(moveInput);
            // }
        }
    }
    public void CombatMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!canAccessCombatMode) {
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You can't use combat mode inside the city.");
                return;
            }
            if(!isEquipping && !isAttacking && !isBlocking){
                animator.SetBool("isCombatMode", !playerStats.isCombatMode);
                playerStats.isCombatMode = !playerStats.isCombatMode;
                isEquipping = !isEquipping;
            }
        }
    }
    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        } 
    }
    public bool GetSubmitPressed() 
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }
    private void SetFacingDirection(Vector2 moveInput)
    {
        if(isAttacking) return;
        if(moveInput.x > 0 && !IsFacingRight)
        {
            // Face the right
            IsFacingRight = true;
        } 
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // Face the left
            IsFacingRight = false;
        }
    }
    public void JournalPressed(InputAction.CallbackContext context)
    {
       
        if (context.performed)
        {
            if(SmithingGameManager.GetInstance().inMiniGame) {
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You can't use open journal while in smithing game.");
                return;
            }
            QuestManager.GetInstance().OpenJournal();
        }
    }
    public void InventoryPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(SmithingGameManager.GetInstance().inMiniGame) {
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You can't use open inventory while in smithing game.");
                return;
            }
            InventoryManager.GetInstance().OpenInventory();
        }
    }
    public void PausePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseManager.GetInstance().OpenPause();
        }
    }
    public void MapPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(SmithingGameManager.GetInstance().inMiniGame) {
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You can't use open map while in smithing game.");
                return;
            }
            MapManager.GetInstance().OpenMap();
        }
    }
    public void ToggleMintingUI(InputAction.CallbackContext context)
    {
        if(!canAccessMap) return;
        if (context.performed)
        {
            PlayerUIManager.GetInstance().ToggleMintingUI();
        }
    }
    public void TogglePremiumShop(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            if(SmithingGameManager.GetInstance().inMiniGame) {
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You can't use open Premium Shop while in smithing game.");
                return;
            }
            PlayerUIManager.GetInstance().TogglePremiumShop();
        }
    }
    public void ToggleChapterSelect(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            if(SmithingGameManager.GetInstance().inMiniGame) {
                PlayerUIManager.GetInstance().SpawnMessage(MType.Error, "You can't use open Chapter Select while in smithing game.");
                return;
            }
            PlayerUIManager.GetInstance().ToggleChapterSelect();
        }
    }
    // Quest Pressed Right
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(isBlocking && playerStats.isCombatMode) return;
            IsRunning = true;
        } 
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }
    public float CurrentMoveSpeed 
    { get 
        {
            if(IsMoving)
            {
                if(IsRunning && playerStats.stamina > 0)
                {
                    return playerStats.runSpeed;
                } else
                {
                    return playerStats.walkSpeed;
                }
            } else 
            {
                // idle speed 0
                return 0;
            }
        }
    }
    
    // When character is moving
    public bool _isMoving = false;

    public bool IsMoving 
    { 
        get 
        {
            return _isMoving;
        } 
        set 
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    } 

    // When character is Running
    public bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }set
        {
                _isRunning = value;
                if(playerStats.stamina < 1) animator.SetBool("isRunning", false);
                else animator.SetBool("isRunning", value);            
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight { get { return _isFacingRight; } private set {
            if(_isFacingRight !=value)
            {
                // Flip the local scale to make the player face the opposite direction
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
    } }

}
