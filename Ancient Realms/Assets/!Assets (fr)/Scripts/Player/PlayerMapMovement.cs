using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[SelectionBase]
public class PlayerMapMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1000f;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Sprite playerIco;
    [SerializeField] Sprite boatIco;
    public InputActionMap mapActionMap;
    private Vector2 movementInput;
    public bool inWater = false;
    public bool isFacingRight = true;
    public bool playerInRange = false;
    private Vector2 _moveDir = Vector2.zero;
    private static PlayerMapMovement Instance;
    void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Player Controller in the scene");
        }
        Instance = this;
    }
    public static PlayerMapMovement GetInstance(){
        return Instance;
    }
    private void Update()
    {
        UpdateIcon();
        if(PlayerController.GetInstance() != null){
            mapActionMap = PlayerController.GetInstance().mapActionMap;
            mapActionMap["Move"].performed += OnMove;
            mapActionMap["Move"].canceled += OnMove;
        }
        if (movementInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (movementInput.x < 0 && isFacingRight)
        {
            Flip();
        }
    }
    private void FixedUpdate()
    {
        // Apply physics-based movement using the Rigidbody2D
        _rb.velocity = movementInput * _moveSpeed * Time.fixedDeltaTime;
    }
    private void UpdateIcon(){
        if(inWater){
            gameObject.GetComponent<SpriteRenderer>().sprite = boatIco;
        }else{
            gameObject.GetComponent<SpriteRenderer>().sprite = playerIco;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed){
            movementInput = context.ReadValue<Vector2>();
        }
        if(context.canceled){
            movementInput = Vector2.zero;
        }
         // Get the input value (Vector2)
    }
    private void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        isFacingRight = !isFacingRight;
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Water"){
            inWater = true;
        }
        if(collider.gameObject.tag == "Location"){
            collider.gameObject.GetComponent<MapTrigger>().OpenTrigger();
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Water"){
            inWater = false;
        }
         if(collider.gameObject.tag == "Location"){
            collider.gameObject.GetComponent<MapTrigger>().CloseTrigger();
        }
    }
}