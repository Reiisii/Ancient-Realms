using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerStats playerStats;
    private Vector2 lastPosition;
    private float distanceMoved;
    private const float moveThreshold = 2f;
    private const float staminaThreshold = 0.5f;
    Vector2 moveInput;
    public bool moveInputActive = false;
    private bool interactPressed = false;
    private bool submitPressed = false;
    Rigidbody2D rb;
    Animator animator;
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
            return;
        }
        
        if(IsMoving){
            if (IsRunning && moveInputActive)
            {
                if (playerStats.stamina > 0)
                {
                    playerStats.stamina -= playerStats.staminaDepletionRate * Time.deltaTime;
                    playerStats.stamina = Mathf.Max(0, playerStats.stamina);
                }
                else
                {
                    IsRunning = false;
                }
            }
        }
        if (moveInputActive)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
            Vector2 currentPosition = transform.position;
            float deltaX = currentPosition.x - lastPosition.x;
            
            if (Mathf.Abs(deltaX) > 0.01f)
            {
                distanceMoved += Mathf.Abs(deltaX);
                if (distanceMoved >= moveThreshold)
                {
                    if (IsRunning == true){
                        QuestManager.GetInstance().UpdateRunGoals(deltaX);
                        distanceMoved = 0f;
                    }else if (!IsRunning){
                        QuestManager.GetInstance().UpdateWalkGoals(deltaX);
                        distanceMoved = 0f;
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
                if(IsRunning)
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
        private set 
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
            animator.SetBool("isRunning", value);
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
