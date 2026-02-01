using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour
{
	private Rigidbody2D rb;

	private float flySpeed;
	[SerializeField] private LayerMask enemyLayer;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
	private int damage = 10;
=======
	private int damage=10;
>>>>>>> Stashed changes
	private Vector3 direction;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		Destroy(gameObject, 1f);
	}

	public void SetDefaultValues(float flySpeed, int damage, Vector3 direction)
	{
		this.flySpeed = flySpeed;
		this.damage = damage;
		this.direction = direction.normalized;

		if (rb == null)
			rb = GetComponent<Rigidbody2D>();
		rb.velocity = this.direction * flySpeed * 0.4f;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Monster"))
		{
			other.GetComponent<Monster>()?.TakeDamage(damage);
			Destroy(gameObject);
		}
		else if (other.CompareTag("Wall"))
		{
			Destroy(gameObject);
		}
	}
}
