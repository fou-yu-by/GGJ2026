using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainPlayer : Singleton<MainPlayer>
{
    private PlayerInput playerInput;
    [HideInInspector]public PlayerStats playerStats;
    private Rigidbody2D rb;
    
    [Header("动画")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isFacingLeft = true; // 默认朝左
    
    [Header("移动")]
    [SerializeField] public Vector2 inputMovement;
    [SerializeField] private float moveSpeed;

    private bool isAttack;
    private bool isPickUp;
    private bool isUseSkill;
    
    [SerializeField] private LayerMask enemyLayer;
    
    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        rb =  GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        moveSpeed = playerStats.moveSpeed.GetValue();
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
        
        playerInput.actions["UseMask"].performed += HandleUseMask;
        playerInput.actions["UseMask"].canceled += HandleUseMask;
        
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
        
        playerInput.actions["UseMask"].performed -= HandleUseMask;
        playerInput.actions["UseMask"].canceled -= CancelUseMask;
        //Event
        
        
    }


    private void OnChangeModifierEvent(object sender, EventArgs e)
    {
        PlayerArgs args = e as PlayerArgs;
        if (args == null) return;
        args._stats.GetValue();
        
        playerStats.UpdateHealthBar();
    }

    #region 用户输入获取
    private void HandleUseMask(InputAction.CallbackContext obj)
    {
        isUseSkill = true;
    }

    private void CancelUseMask(InputAction.CallbackContext obj)
    {
        isUseSkill = false;
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

    private void HandleMove(InputAction.CallbackContext ctx) // AI将onmove改为handlemove
    {
        inputMovement = ctx.ReadValue<Vector2>();
    }

    #endregion

    private void Update()
    {
        Move();
        UpdateAnimation();
        UseCurrentMaskSkill();
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
    
    #region 动画控制
    
    /// <summary>
    /// 更新动画状态
    /// </summary>
    private void UpdateAnimation()
    {
        // 计算速度并设置Speed参数（用于Standing和Walking状态切换）
        float speed = rb.velocity.magnitude;
        animator.SetFloat("Speed", speed);
        
        // 处理左右朝向（只在有水平输入时更新朝向）
        if (inputMovement.x != 0)
        {
            // 输入向左时朝左，输入向右时朝右
            isFacingLeft = inputMovement.x < 0;
            // 默认精灵朝左，所以向右时需要翻转
            spriteRenderer.flipX = !isFacingLeft;
        }
        if (playerStats.currentHealth <= 0)
        {
            PlayDeathAnimation();
        }
    }
    
    /// <summary>
    /// 触发死亡动画
    /// </summary>
    public void PlayDeathAnimation()
    {
        animator.SetBool("isDead", true);
    }
    
    /// <summary>
    /// 获取当前朝向（用于攻击等需要方向的操作）
    /// </summary>
    public Vector2 GetFacingDirection()
    {
        return isFacingLeft ? Vector2.left : Vector2.right;
    }
    
    #endregion
    
    //拾取面具,由面具Trigger触发
    public void PickUpTheMask(MaskBase mask, GameObject hit)
    {
        
        
        if (isPickUp)
        {
            EquipManager.Instance.Mask = mask;
            EquipManager.Instance.UpdateSlotUI();
            
            //拾取后给予特殊效果
            //buff
            GetBuffFromEquipMask(mask);
            
            //开启技能
            this.TriggerEvent("AfterPickUpTheMask");
            
            Destroy(hit);
        }
        
        
    }



    private void GetBuffFromEquipMask(MaskBase mask)
    {
        if (mask.MaskModifiers.Count > 0)
        {
            playerStats.health.modifiers.Clear();
            playerStats.moveSpeed.modifiers.Clear();
            playerStats.damage.modifiers.Clear();
            playerStats.attackDistance.modifiers.Clear();
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

            playerStats.moveSpeed.GetValue();
            playerStats.damage.GetValue();
            playerStats.health.GetValue();
            playerStats.attackDistance.GetValue();
        }
    }

    public void UseCurrentMaskSkill()
    {
        if (isUseSkill)
        {
            if (SkillManager.Instance.canUseFireBall == true)
            {
                isUseSkill = SkillManager.Instance.FireBallSkill.CanUseSkill();
                
            }
            else if (SkillManager.Instance.canUseArrow == true)
            {
                isUseSkill = SkillManager.Instance.ArrowSkill.CanUseSkill();
                
            }
            else if (SkillManager.Instance.canUseJoyHalo == true)
            {
                isUseSkill = SkillManager.Instance.JoyHaloSkill.CanUseSkill();
                
            }
            else if (SkillManager.Instance.canUseMadHalo == true)
            {
                isUseSkill = SkillManager.Instance.MadHaloSkill.CanUseSkill();
                
            }
        }
    }


}
