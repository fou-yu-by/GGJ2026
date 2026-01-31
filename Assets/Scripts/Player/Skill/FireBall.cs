using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform target;
    private float flySpeed;
    [SerializeField] private LayerMask enemyLayer;
    private int damage;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = (target.position - transform.position).normalized * flySpeed;
    }

    public void CacheTarget(Transform target, float flySpeed, int damage)
    {
        this.target = target;
        this.flySpeed = flySpeed;
        this.damage = damage;
    }

    public void TriggerBomb()
    {
        var hit = Physics2D.OverlapCircle(transform.position, 5.0f, enemyLayer);
        if (hit != null)
        {
            //TODO:触发爆炸效果
            var results = Physics2D.OverlapCircleAll(transform.position, 5.0f, enemyLayer);
            if (results.Length <= 0) return;
            foreach (var result in results)
            {
                result.GetComponent<Monster>().TakeDamage(damage);
            }
        }
    }
    
    
}
