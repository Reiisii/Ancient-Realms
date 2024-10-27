using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using System.Linq;
using DG.Tweening;

public class Ally : MonoBehaviour
{
    [Header("Ally Stats")]
    private float maxHP = 100f;
    public float currentHP = 100f;
    public string id = "R-Ally";
    public bool isDead = false;
    public bool isDummy = false;
    public int level = 1;
    public int tier = 1;
    public int equipmentLevel = 1;
    private float maxStamina = 70f;
    public float stamina = 70f;
    private float walkSpeed = 3.2f;
    private float runSpeed = 5f;
    public float staminaDepletionRate = 20f;
    private float staminaRegenRate = 10f;
    private float attackRange = 0.5f;
    private float damage = 1;
    private float armor = 0;
    public int pilumCount = 1;
    public float maxHoldTime = 1f;
    public float holdTime = 0f;
    public List<int> equipments;
    public List<EquipmentSO> equippedSOs;
    private List<Enemy> enemiesInRange = new List<Enemy>();
    public LayerMask enemyLayer;

    [Header("Enemy Controller")]
    public bool invulnerable = false;
    public bool canMove = true;
    public bool IsMoving = false;
    public bool isCombatMode = false;
    public bool isRunning = false;
    public bool isAttacking = false;
    public bool isEquipping = false;
    public bool isBlocking = false;
    public bool isHolding = false;
    public bool isJavelin = false;
    private bool hasBashed = false; 

    [Header("Ally Settings")]
    public Transform contubernium; // Reference to the Contubernium
    private Vector3 targetPosition; 
    public float followDistance = 1.5f; // Distance to maintain from the Contubernium
    public float followSpeed = 2.5f; // Speed of the ally
    private float attackCooldown = 2f;
    private float lastAttackTime; 
    public int row; // Row of the ally in the formation
    public int positionInRow; // Position in the row

    [Header("AI Components")]
    public Animator animator; // Animator for animations
    public AIPath aiPath; // A* pathfinding component
    public AIDestinationSetter aiDestination; // A* destination setter
    public SpriteRenderer spriteRenderer;
    [SerializeField] public SpriteRenderer mainHolster;
    [SerializeField] public SpriteRenderer hand;
    [SerializeField] public SpriteRenderer forearm;
    [SerializeField] public SpriteRenderer mainSlot;
    [SerializeField] public SpriteRenderer shieldSlotFront;
    [SerializeField] public SpriteRenderer shieldSlotBack;
    [SerializeField] public SpriteRenderer shieldSlotHandle;
    [SerializeField] public SpriteRenderer javelinSlot;
    [SerializeField] public JavelinPrefab javelinPrefab;
    [SerializeField] public Transform javelinPoint;
    [SerializeField] public Transform attackPoint;
    private Vector3 previousPosition; // Store previous position for movement detection
    private Vector3 originalScale; // Store the original scale of the ally

    private GameObject tempTargetContainer;
    
    private void Awake()
    {
        CalculateStatsForCurrentLevel();
        previousPosition = transform.position;
        aiPath = GetComponent<AIPath>();
        aiDestination = GetComponent<AIDestinationSetter>();
        aiPath.maxSpeed = followSpeed;
        tempTargetContainer = contubernium.GetComponent<Contubernium>().tempTargetContainer;
        InitializeEquipments();
        // Store the original scale
        originalScale = transform.localScale;
    }
    private void Start(){
        bool contuberniumFacingRight = contubernium.localScale.x > 0;
        IsFacingRight = contuberniumFacingRight;

    }
    [ContextMenu("Die")]
    public void Die(){
        currentHP = 0;
    }
    private void Update()
    {
        RegenStamina();
        if (isHolding)
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Min(holdTime, maxHoldTime); // Cap the hold time to the max hold time
        }
        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            animator.Play("Death");
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            // Notify the Contubernium that this ally has died
            Contubernium.Instance.HandleAllyDeath(this);
        }else if (canMove) // Only execute movement logic if canMove is true
        {
            aiDestination.target = CreateTempTarget(targetPosition);
            aiPath.maxSpeed = followSpeed;
            float distanceToContubernium = Vector3.Distance(transform.position, contubernium.position);
            SetRunningAnimation(distanceToContubernium);
            if (IsMoving)
            {
                SetFacingDirection(aiPath.velocity); // Face the direction of movement
            }
            else
            {
                FaceContubernium(); // Align with the Contubernium's direction when not moving
            }
        }
        CheckForEnemiesAndAttack();
    }

    void FixedUpdate(){
        OnMove();
    }
    public void CombatToggle(){
        if(!isEquipping && !isAttacking && !isBlocking){
                animator.SetBool("isCombatMode", !isCombatMode);
                isCombatMode = !isCombatMode;
                isEquipping = !isEquipping;
        }
    }
    private void ThrowPilum()
    {
        PlayerStats playerStats = PlayerStats.GetInstance();
        PlayerController playerController = PlayerController.GetInstance();
        float maxDamage = equippedSOs[6].baseDamage * 1.5f;

        // Calculate the throw force and corresponding damage based on hold time
        float damage = Mathf.Lerp(0, maxDamage, playerController.holdTime / playerStats.maxHoldTime);
        stamina -= 10f;
        // Instantiate the pilum
        JavelinPrefab pilum = Instantiate(javelinPrefab, javelinPoint.position, javelinPoint.rotation);
        // Apply force to the pilum in an arching trajectory
        Rigidbody2D rb = pilum.GetComponent<Rigidbody2D>();
        float throwForce = 1f / 1f * playerStats.maxThrowForce;

        float throwAngle = 22f; // Lower angle for a flatter trajectory

        // Adjust the force direction based on whether the player is facing right or left
        Vector2 forceDirection = IsFacingRight ? Vector2.right : Vector2.left;

        Vector2 force = new Vector2(
            forceDirection.x * throwForce * Mathf.Cos(throwAngle * Mathf.Deg2Rad), 
            throwForce * Mathf.Sin(throwAngle * Mathf.Deg2Rad) * 0.5f // Reduce the vertical component
        );

        pilum.SetDamage(damage);
        holdTime = 0f;
        pilumCount -= 1;
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
        isHolding = false;
    }
    private void SetRunningAnimation(float distance)
    {
        if (distance > 20f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        animator.SetBool("isRunning", isRunning);
    }

    private Transform CreateTempTarget(Vector3 position)
    {
        GameObject tempTarget = new GameObject("TempTarget");
        tempTarget.transform.position = position;
        tempTarget.transform.parent = tempTargetContainer.transform; // Set parent to "TempTargets"
        return tempTarget.transform;
    }

    public void OnMove()
    {
        float distanceMoved = Vector3.Distance(transform.position, previousPosition);
        
        // Check if the object has moved more than the threshold value
        if (distanceMoved > 0.01f)
        {
            IsMoving = true;
            animator.SetBool("isMoving", true);
        }
        else
        {
            IsMoving = false;
            animator.SetBool("isMoving", false);
        }

        previousPosition = transform.position;
    }



    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void SetSpriteOrder(int order)
    {
        if(isDead) return;
        spriteRenderer.sortingOrder = order;
        mainHolster.sortingOrder = order + 3;
        mainSlot.sortingOrder = order + 3;
        shieldSlotBack.sortingOrder = order - 1;
        shieldSlotFront.sortingOrder = order + 2;
        shieldSlotHandle.sortingOrder = order + 1;
        javelinSlot.sortingOrder = order + 3;
        hand.sortingOrder = order + 4;
        forearm.sortingOrder = order + 4;
    }
    private void SetFacingDirection(Vector2 velocity)
    {
        if (isAttacking) return; // Don't flip while attacking

        // Check the velocity to determine the direction
        bool shouldFaceRight = velocity.x > 0.1f; // Adjust the threshold to handle small values of velocity

        // Face the movement direction
        if (shouldFaceRight && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (!shouldFaceRight && IsFacingRight && velocity.x < -0.1f)
        {
            IsFacingRight = false;
        }
    }
    public void SetFacingDirection(bool isR){
        if(isR){
            IsFacingRight = true;
        }else{
            IsFacingRight = false;
        }
    }
    private void FaceContubernium()
    {
        // Check the Contubernium's facing direction
        bool contuberniumFacingRight = contubernium.localScale.x > 0;

        // Face the same direction as the Contubernium
        if (contuberniumFacingRight && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (!contuberniumFacingRight && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public bool IsFacingRight
    {
        get 
        {
            // Ally is facing right if the localScale.x is positive
            return transform.localScale.x > 0;
        }
        private set 
        {
            // Flip the ally only if needed
            if ((value && transform.localScale.x < 0) || (!value && transform.localScale.x > 0))
            {
                // Flip the local scale by multiplying x by -1
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
    }
    
    public void InitializeEquipments(){
        List<EquipmentSO> newList = new List<EquipmentSO>();
        foreach(int equipment in equipments){
            EquipmentSO equipmentSO = AccountManager.Instance.equipments.Where(eq => eq.equipmentId == equipment).FirstOrDefault();
            EquipmentSO copiedEquipmentSO = equipmentSO.CreateCopy(equipment, tier, equipmentLevel);
            newList.Add(copiedEquipmentSO);
        }
        equippedSOs = newList;
        int j = 0;
        float sumArmor = 0;
        damage = 1;
        mainHolster.sprite = equippedSOs[4].front;
        mainSlot.sprite = equippedSOs[4].front;
        shieldSlotFront.sprite = equippedSOs[5].front;
        shieldSlotBack.sprite = equippedSOs[5].back;
        javelinSlot.sprite = equippedSOs[6].front;
        foreach(EquipmentSO equipment in equippedSOs){
            if(equipment && equipment.equipmentType == EquipmentEnum.Armor){
                sumArmor += equipment.baseArmor;
            }
            if(equipment && equipment.equipmentType == EquipmentEnum.Weapon && (equipment.weaponType == WeaponType.Sword ||  equipment.weaponType == WeaponType.Spear) && j == 4){
                damage = equipment.baseDamage;
                attackRange = equipment.attackRange;
            }
            j++;
        }
        armor = sumArmor;
    }
    void CheckForEnemiesAndAttack()
    {
        // Use Physics2D.OverlapCircleAll to detect enemies in the attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange + 0.2f, enemyLayer);
        
        // If there are enemies within range
        if (hitEnemies.Length > 0)
        {
            // Get a list of Enemy components from the hit colliders
            List<Enemy> enemiesInRange = hitEnemies
                .Select(collider => collider.GetComponent<Enemy>())
                .Where(enemy => enemy != null && !enemy.isDead) // Ensure valid and alive enemies
                .ToList();

            if (enemiesInRange.Count > 0)
            {
                // Find the target enemy (e.g., the one with the lowest health)
                Enemy targetEnemy = enemiesInRange.OrderBy(enemy => enemy.currentHP).FirstOrDefault();

                if (targetEnemy != null && !targetEnemy.isDead)
                {
                    if (stamina < 20f && !isBlocking)
                    {
                        StartBlocking(); // Initiate block
                    }
                    else if (!isAttacking)
                    {
                        if (Time.time < lastAttackTime + attackCooldown || stamina < 20) return;
                        PerformAttack(targetEnemy); // Attack if not already attacking
                    }
                }
            }
        }
    }
    void StartBlocking()
    {
        isBlocking = true;
        hasBashed = false;
        animator.SetBool("isBlocking", true); // Play block animation

        // Block for a fixed time or until stamina depletes
        StartCoroutine(BlockCoroutine());
    }
    void StopBlocking()
    {
        isBlocking = false;
        animator.SetBool("isBlocking", false);// Return to idle
    }

    IEnumerator BlockCoroutine()
    {
        float blockTime = 2f; // Duration of blocking
        float blockStartTime = Time.time;

        while (Time.time < blockStartTime + blockTime && stamina > 0f)
        {
            stamina = Mathf.Max(0, stamina + (staminaRegenRate * 0.25f) * Time.deltaTime);
            if (Random.value > 0.8f && !hasBashed) // 20% chance, only if enough stamina
            {
                hasBashed = true;
                PerformShieldBash();
            }
            yield return null; // Wait for next frame
        }
        StopBlocking(); // Stop when out of stamina or time is up
    }
    void ApplyDamage(){
        Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        enemiesInRange.Clear();
        foreach(Collider2D enemy in hitEnemies){
            enemiesInRange.Add(enemy.GetComponent<Enemy>());
        }
        Enemy targetEnemy = enemiesInRange.OrderByDescending(enemy => enemy.currentHP).FirstOrDefault();
        if (targetEnemy != null) {
            targetEnemy.TakeDamage(damage);
        }
        enemiesInRange.Clear();
    }
    void ApplyBash(){
        Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        enemiesInRange.Clear();
        foreach(Collider2D enemy in hitEnemies){
            enemiesInRange.Add(enemy.GetComponent<Enemy>());
        }
        Enemy targetEnemy = enemiesInRange.OrderByDescending(enemy => enemy.currentHP).FirstOrDefault();
        if (targetEnemy != null) {
            targetEnemy.MoveBackStun();
        }
        enemiesInRange.Clear();
    }
    private void PerformAttack(Enemy target)
    {
        if(stamina < 10) return;
        isAttacking = true;
        lastAttackTime = Time.time;
        stamina -= 10f;
        // Optionally, add a cooldown or delay between attacks
    }
    private void PerformShieldBash()
    {
        // Play the shield bash animation
        if(stamina > 20) return;
        isAttacking = true; // You need a shield bash animation in the Animator
    }
    public void TakeDamage(float damage){
            float newDamage = Utilities.CalculateDamage(damage,  armor);
            if(invulnerable){
                return;
            }else if(isBlocking){
                currentHP -= Utilities.CalculateDamage(damage,  armor, true);
            }else{
                currentHP -= newDamage;
            }
    }
    public void MoveFront()
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
        gameObject.transform.DOMove(newPosition, moveDuration).SetEase(Ease.OutQuad);
        
    }
    private void RegenStamina(){
        if (!isBlocking && !isAttacking)
        {
            if(isCombatMode && IsMoving) {
                stamina = Mathf.Max(0, stamina - (staminaDepletionRate * 0.25f) * Time.deltaTime);
                return;
            }else if(isCombatMode){
                stamina = Mathf.Min(maxStamina, stamina + (staminaRegenRate - (staminaRegenRate * 0.50f)) * Time.deltaTime);
            }else{
                stamina = Mathf.Min(maxStamina, stamina + staminaRegenRate * Time.deltaTime);
            }
        }
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
            if (gameObject == null) return;  // Stops movement if the object is destroyed
        });;
        
    }
    public void MoveBackStun()
    {
        if (gameObject == null) return;
        Vector2 cP = gameObject.transform.position;
        float moveDistance = -0.4f;
        Vector2 moveDirection = IsFacingRight ? Vector2.right : Vector2.left;
        Vector2 newPosition = cP + moveDirection * moveDistance;
        float moveDuration = 0.2f;

        canMove = false;

        gameObject.transform.DOMove(new Vector3(newPosition.x, newPosition.y, transform.position.z), moveDuration)
            .SetEase(Ease.OutQuad)
            .OnKill(() => {
                if (gameObject == null) return;  // Prevents further movement if destroyed
            })
            .OnComplete(() => {
                if (gameObject == null) return;
                DOVirtual.DelayedCall(3f, () => 
                {
                    if (gameObject == null) return;
                    canMove = true;
                });
            });
    }

    private void CalculateStatsForCurrentLevel()
    {
        // Calculate the stats based on the current level without incrementing it
        maxHP = 100f * Mathf.Pow(1.02f, level - 1); // Assuming initial maxHP is 100
        currentHP = maxHP;
        maxStamina = 70f * Mathf.Pow(1.03f, level - 1); // Assuming initial maxStamina is 70
        staminaRegenRate = 10f * Mathf.Pow(1.03f, level - 1); // Assuming initial staminaRegenRate is 10
    }
}
