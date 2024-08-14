using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerStats playerStats;
    [SerializeField] public Transform attackPoint;
    public LayerMask enemyLayer;

    private Vector2 lastPosition;
    private float distanceMoved;
    private const float moveThreshold = 2f;
    Vector2 moveInput;
    public bool moveInputActive = false;
    private bool interactPressed = false;
    private bool submitPressed = false;
    public bool isAttacking = false;
    public bool canWalk;
    public bool isBlocking;
    Rigidbody2D rb;
    public Animator animator;
    private static PlayerController Instance;
    private void Awake()
    {
        if(Instance != null){
            Debug.LogWarning("Found more than one Player Controller in the scene");
        }
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    private void Start(){
        lastPosition = transform.position;
    }
    public static PlayerController GetInstance(){
        return Instance;
    }
    public bool GetInteractPressed() 
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying || interactPressed)
        {
            // Stop player movement during dialogue or interaction
            rb.velocity = Vector2.zero;
            IsRunning = false;
            IsMoving = false;
            animator.SetBool("isCombatMode", false);
            playerStats.isCombatMode = false;
            return;
        }
        
        if(IsMoving){
            if (IsRunning && moveInputActive)
            {
                if (playerStats.stamina > 0 && playerStats.toggleStamina == true)
                {
                    playerStats.stamina -= playerStats.staminaDepletionRate * Time.deltaTime;
                    playerStats.stamina = Mathf.Max(0, playerStats.stamina);
                }
                else if(playerStats.stamina < 1){
                    IsRunning = false;
                }
            }
        }
        if(canWalk){
            if (moveInputActive)
            {
                IsMoving = true;
                SetFacingDirection(moveInput);
                rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
                Vector2 currentPosition = transform.position;
                float deltaX = currentPosition.x - lastPosition.x;
                
                if (Mathf.Abs(deltaX) > 0.01f)
                {
                    distanceMoved += Mathf.Abs(deltaX);
                    if (distanceMoved >= moveThreshold)
                    {
                        if (IsRunning == true){
                            List<QuestSO> quest = playerStats.activeQuests.ToList();
                            foreach(QuestSO q in quest){
                                if(q.goals[q.currentGoal].goalType == GoalTypeEnum.RunLeft || q.goals[q.currentGoal].goalType == GoalTypeEnum.RunRight){
                                    QuestManager.GetInstance().UpdateRunGoals(deltaX);
                                    distanceMoved = 0f;
                                } 
                            }
                            
                            distanceMoved = 0f;
                        }else if (!IsRunning){
                            List<QuestSO> quest = playerStats.activeQuests.ToList();
                            foreach(QuestSO q in quest){
                                if(q.goals[q.currentGoal].goalType == GoalTypeEnum.WalkLeft || q.goals[q.currentGoal].goalType == GoalTypeEnum.WalkRight){
                                    QuestManager.GetInstance().UpdateWalkGoals(deltaX);
                                    distanceMoved = 0f;
                                } 
                            }
                            
                        }
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
    }
    public void Attack(InputAction.CallbackContext context){
        if(context.performed){
            if(playerStats.stamina < 10) return;
            if(PlayerStats.GetInstance().isCombatMode && !isAttacking && IsRunning == false){
                isAttacking = true;
                playerStats.stamina -= 10f;
            }else{
                return;
            }
        }
    }
    void Applydamage(){
        Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerStats.attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies){
            enemy.GetComponent<Enemy>().TakeDamage(playerStats.attack);
        }
    }
    void OnDrawGizmosSelected(){
        if(attackPoint == null) return;
        Gizmos .DrawWireSphere(attackPoint.position, playerStats.attackRange);
    }
    public void Block(InputAction.CallbackContext context){
        if(context.performed){
            if(PlayerStats.GetInstance().isCombatMode && IsRunning == false){
                animator.SetBool("isBlocking", true);
                playerStats.stamina -= 5f * Time.deltaTime;
            }
        }else if (context.canceled)
        {
            animator.SetBool("isBlocking", false);
        }
    }
    void MoveCharacterAfterDelay()
    {
        // Get the character's current position
        Vector2 cP = PlayerStats.GetInstance().gameObject.transform.position;

        // Define the movement distance (adjust as needed)
        float moveDistance = 0.6f;

        // Calculate the new position based on the direction the character is facing
        Vector2 moveDirection = IsFacingRight ? Vector2.right : Vector2.left;

        Vector2 newPosition = cP + moveDirection * moveDistance;

        // Define the duration for the smooth movement (adjust as needed)
        float moveDuration = 0.1f;

        // Use DOTween to smoothly move the character to the new position
        PlayerStats.GetInstance().gameObject.transform.DOMove(newPosition, moveDuration).SetEase(Ease.OutQuad);
        
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
            IsMoving = false;
            IsRunning = false;
        }
        else if (context.canceled)
        {
            interactPressed = false;
            // Resume movement if moveInput is still active
            if (moveInputActive)
            {
                IsMoving = true;
                SetFacingDirection(moveInput);
            }
        }
    }
    public void CombatMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsRunning = false;
            IsMoving = false;
            canWalk = false;
            animator.SetBool("isCombatMode", !playerStats.isCombatMode);
            playerStats.isCombatMode = !playerStats.isCombatMode;
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
        } private set
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
