using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    
    private float flySpeed;
    [SerializeField] private LayerMask enemyLayer;
    private int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDefaultValues(float flySpeed, int damage)
    {
        this.flySpeed = flySpeed;
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().TakeDamage(damage);
        }
        else if (other.CompareTag("Wall")) //TODO:射中墙体之后留在墙体上
        {
            transform.SetParent(other.transform);
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
        }
    }
}
