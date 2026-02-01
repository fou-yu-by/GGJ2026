using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        Destroy(gameObject, 4);
    }

    private void Update()
    {
        if(target == null) return;
        else{ rb.velocity = (target.position - transform.position).normalized * flySpeed;}
        // Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        // transform.LookAt(targetPosition);
        TriggerBomb();
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
        var hit = Physics2D.OverlapCircle(transform.position, 2, enemyLayer);
        if (hit != null)
        {
            //TODO:触发爆炸效果
            var results = Physics2D.OverlapCircleAll(transform.position, damageRange, enemyLayer);
            if (results.Length <= 0) return;
            foreach (var result in results)
            {
                result.GetComponent<Monster>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
