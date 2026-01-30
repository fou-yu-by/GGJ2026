using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 inputMovement;
    
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed;
    
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb =  GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].Enable();
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].Disable();
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        inputMovement = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        if (inputMovement != Vector2.zero)
        {
            rb.velocity = inputMovement * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
