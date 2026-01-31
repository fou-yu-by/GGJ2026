using UnityEngine;

public abstract class Monster : MonoBehaviour
{
	[Header("Stats")]
	[SerializeField] protected int maxHp = 100;
	[SerializeField] protected int attackPower = 10;
	[SerializeField] protected float collisionCooldown = 1f;

	protected int currentHp;
	protected GameObject player;
	protected PlayerStats playerStats;
	protected float collisionCooldownTimer = 0f;

	protected virtual void Awake()
	{
		currentHp = Mathf.Clamp(currentHp == 0 ? maxHp : currentHp, 0, maxHp);
		CachePlayer();
	}

	protected virtual void Start()
	{
		if (player == null)
		{
			CachePlayer();
		}
	}

	protected void CachePlayer()
	{
		player = GameObject.Find("Player");
		if (player == null)
		{
			Debug.LogWarning("Monster: 未找到名为 'Player' 的对象。");
		}
		else
		{
			playerStats = player.GetComponent<PlayerStats>();
			if (playerStats == null)
			{
				Debug.LogWarning("Monster: Player 上未找到 PlayerStats 组件。");
			}
		}
	}

	public GameObject Player => player;
	public int MaxHp => maxHp;
	public int CurrentHp => currentHp;
	public int AttackPower => attackPower;
	public Vector2 PositionXY => new Vector2(transform.position.x, transform.position.y);

	protected virtual void Update()
	{
		Move();
	}

	protected virtual void FixedUpdate()
	{
		if (collisionCooldownTimer > 0f)
		{
			collisionCooldownTimer -= Time.fixedDeltaTime;
		}
	}

	protected abstract void Move();

	protected abstract void OnPlayerCollision(GameObject playerObject);

	protected virtual void Attack()
	{
	}

	public virtual void TakeDamage(int damage)
	{
		currentHp -= damage;
		Debug.Log($"Monster: 受到 {damage} 点伤害，剩余血量 {currentHp}");
		if (currentHp <= 0)
		{
			Die();
		}
	}

	protected virtual void Die()
	{
		Debug.Log("Monster: 死亡");
		Destroy(gameObject);
	}

	protected virtual void OnCollisionStay2D(Collision2D collision)
	{
		TryHandlePlayerCollision(collision.gameObject);
	}

	protected virtual void OnTriggerStay2D(Collider2D collider)
	{
		TryHandlePlayerCollision(collider.gameObject);
	}

	private void TryHandlePlayerCollision(GameObject other)
	{
		if (other == null)
		{
			return;
		}

		if (player == null)
		{
			CachePlayer();
		}

		if (other == player && collisionCooldownTimer <= 0f)
		{
			collisionCooldownTimer = collisionCooldown;
			OnPlayerCollision(other);
		}
	}
}
