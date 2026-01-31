using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [HideInInspector]public Rigidbody2D rb;
    private Transform target;
    private float flySpeed;
    [SerializeField] private LayerMask enemyLayer;
    private int damage;
    private float damageRange;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(target == null) return;
        else{ rb.velocity = (target.position - transform.position).normalized * flySpeed;}
    }

    public void CacheTarget(Transform target, float flySpeed, int damage, float damageRange)
    {
        this.target = target;
        this.flySpeed = flySpeed;
        this.damage = damage;
        this.damageRange = damageRange;
    }

    public void SetDefaultValue(float flySpeed, int damage)
    {
        this.target = null;
        this.flySpeed = flySpeed;
        this.damage = damage;
    }
    
    public void TriggerBomb()
    {
        var hit = Physics2D.OverlapCircle(transform.position, damageRange, enemyLayer);
        if (hit != null)
        {
            //TODO:触发爆炸效果
            var results = Physics2D.OverlapCircleAll(transform.position, damageRange, enemyLayer);
            if (results.Length <= 0) return;
            foreach (var result in results)
            {
                result.GetComponent<Monster>().TakeDamage(damage);
            }
        }
    }
    
    
}
