using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    Vector2 moveInput;
    public bool interactPressed = false;
    public float CurrentMoveSpeed 
    { get 
        {
            if(IsMoving)
            {
                if(IsRunning)
                {
                    return runSpeed;
                } else
                {
                    return walkSpeed;
                }
            } else 
            {
                // idle speed 0
                return 0;
            }
        }
    }
    
    // When character is moving
    private bool _isMoving = false;

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
    private bool _isRunning = false;
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

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public bool GetInteractPressed() 
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
    }
    public void OnMove(InputAction.CallbackContext context) 
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }
    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        } 
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
}
