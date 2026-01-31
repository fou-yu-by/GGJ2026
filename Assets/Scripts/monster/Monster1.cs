using UnityEngine;

public class Monster1 : Monster
{
	[Header("Movement")]
	[SerializeField] protected float moveSpeed = 5f;
	[SerializeField] protected float movediffrange = 1f;
	private float actualMoveSpeed;

	protected override void Awake()
	{
		base.Awake();
		actualMoveSpeed = moveSpeed + Random.Range(-movediffrange, movediffrange);
	}

	protected override void Move()
	{
		if (player == null)
		{
			return;
		}

		Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
		transform.Translate(directionToPlayer * actualMoveSpeed * Time.deltaTime);
	}

	protected override void OnPlayerCollision(GameObject playerObject)
	{
		Debug.Log("Monster1: 与玩家发生碰撞");
		Debug.Log($"Monster1: 对玩家进行攻击，攻击力 = {attackPower}");
		if (playerStats != null)
		{
			playerStats.TakeDamage(attackPower);
		}
	}

	protected override void Attack()
	{

		// Debug.Log($"Monster1: 对玩家进行攻击，攻击力 = {attackPower}");
	}
}
