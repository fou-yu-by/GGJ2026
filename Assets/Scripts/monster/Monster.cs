using UnityEngine;

public abstract class Monster : MonoBehaviour
{
	[Header("Stats")]
	[SerializeField] protected int maxHp = 100;
	[SerializeField] protected int attackPower = 10;
	[SerializeField] protected float collisionCooldown = 1f;

	protected int currentHp;
	protected GameObject player;
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
			Debug.LogWarning("Monster: 未找到名为 'player' 的对象。");
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

	protected virtual void OnCollisionEnter2D(Collision2D collision)
	{
		TryHandlePlayerCollision(collision.gameObject);
	}

	protected virtual void OnTriggerEnter2D(Collider2D collider)
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
