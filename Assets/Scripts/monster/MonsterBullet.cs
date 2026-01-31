using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterBullet : MonoBehaviour
{
	private Vector2 direction;
	private float speed;
	private float maxRange;
	private int damage;
	private Vector2 startPosition;
	private bool isInitialized = false;
	private Rigidbody2D rb;

    private bool isAggroToMonsters;
	[Header("Settings")]
	[SerializeField] private LayerMask obstacleLayer;
	[SerializeField] private bool destroyOnObstacle = true;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		if (rb == null)
		{
			rb = gameObject.AddComponent<Rigidbody2D>();
		}
		// 设置为不受重力影响
		rb.gravityScale = 0f;
		rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // 防止高速穿透
	}

	/// <summary>
	/// 初始化子弹参数
	/// </summary>
	public void Initialize(Vector2 direction, float speed, float maxRange, int damage, LayerMask? obstacle = null, bool isAggroToMonsters = false)
	{
		this.direction = direction.normalized;
		this.speed = speed;
		this.maxRange = maxRange;
		this.damage = damage;
		this.startPosition = transform.position;
		if (obstacle.HasValue)
		{
			this.obstacleLayer = obstacle.Value;
		}
		this.isAggroToMonsters = isAggroToMonsters;
		// 使用 Rigidbody2D 速度移动，这样物理系统会正确检测碰撞
		if (rb != null)
		{
			rb.velocity = this.direction * this.speed;
		}
		
		this.isInitialized = true;
	}

	private void Update()
	{
		if (!isInitialized) return;

		// 检查是否超出射程
		float distanceTraveled = Vector2.Distance(startPosition, transform.position);
		if (distanceTraveled >= maxRange)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
        if (isAggroToMonsters)
        {
            if (other.CompareTag("Monster"))
            {
                other.GetComponent<Monster>()?.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }
		// 检查是否击中玩家
		if (other.CompareTag("Player"))
		{
			PlayerStats playerStats = other.GetComponent<PlayerStats>();
			if (playerStats != null)
			{
				playerStats.TakeDamage(damage);
				Debug.Log($"MonsterBullet: 击中玩家，造成 {damage} 点伤害");
			}
			Destroy(gameObject);
			return;
		}

		// 检查是否击中障碍物（wall标签或obstacleLayer）
		if (destroyOnObstacle)
		{
			if (other.CompareTag("wall") || IsInLayerMask(other.gameObject.layer, obstacleLayer))
			{
				Destroy(gameObject);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// 检查是否击中玩家
		if (collision.gameObject.CompareTag("Player"))
		{
			PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
			if (playerStats != null)
			{
				playerStats.TakeDamage(damage);
				Debug.Log($"MonsterBullet: 击中玩家，造成 {damage} 点伤害");
			}
			Destroy(gameObject);
			return;
		}

		// 击中墙壁或其他物体时销毁
		if (destroyOnObstacle)
		{
			if (IsInLayerMask(collision.gameObject.layer, obstacleLayer))
			{
				Destroy(gameObject);
			}
		}
	}

	private bool IsInLayerMask(int layer, LayerMask layerMask)
	{
		return (layerMask.value & (1 << layer)) != 0;
	}
}
