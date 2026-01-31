using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainPlayer : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    
    
    [Header("移动")]
    private Vector2 inputMovement;
    [SerializeField] private float moveSpeed;

    public bool isAttack;
    [SerializeField] private LayerMask enemyLayer;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb =  GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].Enable();
        playerInput.actions["Move"].performed += HandleMove;
        playerInput.actions["Move"].canceled += HandleMove;

        playerInput.actions["Attack"].started += HandleAttack;
    }


    private void OnDisable()
    {
        playerInput.actions["Move"].Disable();
        playerInput.actions["Move"].performed -= HandleMove;
        playerInput.actions["Move"].canceled -= HandleMove;
    }
    private void HandleAttack(InputAction.CallbackContext ctx)
    {
        isAttack = true;
        Vector3 attackDirection = inputMovement.normalized;
        
        Debug.DrawLine(transform.position,attackDirection * playerStats.attackDistance.GetValue(), Color.red);
        var hits = Physics2D.RaycastAll(transform.position, attackDirection, enemyLayer);
        //hits里的每一个enemy执行受伤逻辑
        foreach (var enemy in hits)
        {
            enemy.transform.GetComponent<Monster>().TakeDamage(playerStats.damage.GetValue());
        }
        
    }

    private void HandleMove(InputAction.CallbackContext ctx)// AI将onmove改为handlemove
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
