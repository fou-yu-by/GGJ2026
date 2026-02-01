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
     
    private Vector3 direction;
    private void Awake()
    {
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = MainPlayer.Instance.inputMovement * flySpeed;
    }
    

    public void SetDefaultValues(float flySpeed, int damage, Vector3 direction)
    {
        this.flySpeed = flySpeed;
        this.damage = damage;
        this.direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<Monster>().TakeDamage(damage);
        }
        // else if (other.CompareTag("Wall")) //TODO:射中墙体之后留在墙体上
        // {
        //     transform.SetParent(other.transform);
        //     rb.bodyType = RigidbodyType2D.Kinematic;
        //     rb.velocity = Vector2.zero;
        // }
    }
}
