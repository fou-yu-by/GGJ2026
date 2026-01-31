using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class MainPlayer : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    
    [Header("移动")]
    private Vector2 inputMovement;
    [SerializeField] private float moveSpeed;

    private bool isAttack;
    private bool isPickUp;
    
    [SerializeField] private LayerMask enemyLayer;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb =  GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();

    }


    private void OnEnable()
    {
        //inputSystem
        playerInput.actions["Move"].Enable();
        playerInput.actions["Move"].performed += HandleMove;
        playerInput.actions["Move"].canceled += HandleMove;

        playerInput.actions["Attack"].started += HandleAttack;

        playerInput.actions["SwitchMask"].performed += HandlePickUp;
        playerInput.actions["SwitchMask"].canceled += CancelPickUp;
        
        //Event
        EventManager.Instance.AddListener("ChangeModifierEvent", OnChangeModifierEvent);
        
    }




    private void OnDisable()
    {
        //InputSystem
        playerInput.actions["Move"].Disable();
        playerInput.actions["Move"].performed -= HandleMove;
        playerInput.actions["Move"].canceled -= HandleMove;
        
        playerInput.actions["SwitchMask"].performed -= HandlePickUp;
        playerInput.actions["SwitchMask"].canceled -= CancelPickUp;
        
        //Event
        
        
    }
    private void OnChangeModifierEvent(object sender, EventArgs e)
    {
        PlayerArgs args = e as PlayerArgs;
        if (args == null) return;
        args._stats.GetValue();
        
        playerStats.UpdateHealthBar();
    }
    private void CancelPickUp(InputAction.CallbackContext ctx)
    {
        isPickUp = false;
    }

    private void HandlePickUp(InputAction.CallbackContext ctx)
    {
        isPickUp = true;
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
            enemy.transform.GetComponent<Monster>()?.TakeDamage(playerStats.damage.GetValue());
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
    
    //拾取面具,由面具Trigger触发
    public void PickUpTheMask(MaskBase mask)
    {
        
        
        if (isPickUp)
        {
            EquipManager.Instance.Mask = mask;
            EquipManager.Instance.UpdateSlotUI();
            
            //拾取后给予特殊效果
            //buff
            GetBuffFromEquipMask(mask);
            
            //TODO开启技能
            this.TriggerEvent("AfterPickUpTheMask");
            
        }
        
    }



    private void GetBuffFromEquipMask(MaskBase mask)
    {
        if (mask.MaskModifiers.Count > 0)
        {
            //即面具存在buff效果
            foreach (var modifier in mask.MaskModifiers)
            {
                switch (modifier.modifierName)
                {
                    case "damage":
                        playerStats.IncreaseStatsByBuff(modifier.modifierValue, modifier.modifierDuration,
                            playerStats.damage);
                        break;
                    case "health":
                        playerStats.IncreaseStatsByBuff(modifier.modifierValue, modifier.modifierDuration,
                            playerStats.health);
                        break;
                    case "moveSpeed":
                        playerStats.IncreaseStatsByBuff(modifier.modifierValue, modifier.modifierDuration,
                            playerStats.moveSpeed);
                        break;
                    case "attackDistance":
                        playerStats.IncreaseStatsByBuff(modifier.modifierValue, modifier.modifierDuration,
                            playerStats.attackDistance);
                        break;
                }

                
            }
        }
    }



}
