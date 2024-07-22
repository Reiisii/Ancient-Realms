using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1000f;
    [SerializeField] Rigidbody2D _rb;

    private Vector2 _moveDir = Vector2.zero;

    private void Update()
    {
        GatherInput();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }

    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");

        Debug.Log(_moveDir);
    } 

    private void MovementUpdate()
    {
        _rb.velocity = _moveDir * _moveSpeed * Time.fixedDeltaTime;
    }
}