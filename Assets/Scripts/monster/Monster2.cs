using UnityEngine;

public class Monster2 : Monster
{
	[Header("Movement")]
	[SerializeField] protected float moveSpeed = 3f;
	[SerializeField] protected float movediffrange = 0.5f;
	
	[Header("Distance Control")]
	[SerializeField] protected float preferredDistance = 8f;   // 理想距离
	[SerializeField] protected float minDistance = 5f;         // 最小距离（太近会后退）
	[SerializeField] protected float maxDistance = 12f;        // 最大距离（太远会靠近）
	[SerializeField] protected float distanceTolerance = 1f;   // 距离容差
	
	[Header("Shooting")]
	[SerializeField] protected GameObject bulletPrefab;        // 子弹预制体
	[SerializeField] protected float bulletSpeed = 10f;        // 子弹速度
	[SerializeField] protected float bulletRange = 15f;        // 子弹射程
	[SerializeField] protected float fireRate = 1.5f;          // 射击间隔
	[SerializeField] protected Transform firePoint;            // 发射点（可选）
	
	[Header("Obstacle Avoidance")]
	[SerializeField] protected float obstacleCheckDistance = 2f;
	[SerializeField] protected float avoidanceStrength = 2f;
	[SerializeField] protected LayerMask obstacleLayer;        // 障碍物层
	
	private float actualMoveSpeed;
	private float fireTimer = 0f;
	private bool hasLineOfSight = false;
	private Collider2D myCollider;  // 缓存自己的碰撞体
    [SerializeField] protected Collider2D sonCollider;
	protected override void Awake()
	{
		base.Awake();
		actualMoveSpeed = moveSpeed + Random.Range(-movediffrange, movediffrange);
		myCollider = GetComponent<Collider2D>();  // 缓存碰撞体
	}

	protected override void Update()
	{
		base.Update();
		
		if (!isActivated || player == null || isDizzy) return;
		
		// 更新射击计时器
		fireTimer -= Time.deltaTime;
		
		// 检查视线
		hasLineOfSight = CheckLineOfSight();
		
		// 如果有视线且在射程内，尝试射击
		float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
		if (hasLineOfSight && distanceToPlayer <= bulletRange && fireTimer <= 0f)
		{
			Shoot();
			fireTimer = fireRate;
		}
	}

	protected override void Move()
	{
		if (player == null || isDizzy) return;

		Vector2 currentPos = transform.position;
		Vector2 playerPos = player.transform.position;
		float distanceToPlayer = Vector2.Distance(currentPos, playerPos);
		
		Vector2 directionToPlayer = (playerPos - currentPos).normalized;
		Vector2 moveDirection = Vector2.zero;

		// 当目标是怪物时（互相攻击状态），使用简单的直线移动
		if (isAggroToMonsters && player.CompareTag("Monster"))
		{
			moveDirection = directionToPlayer;
			transform.Translate(moveDirection * actualMoveSpeed * Time.deltaTime);
			return;
		}

		// 根据距离决定移动方向
		if (distanceToPlayer < minDistance)
		{
			// 太近了，后退
			moveDirection = -directionToPlayer;
		}
		else if (distanceToPlayer > maxDistance)
		{
			// 太远了，靠近
			moveDirection = directionToPlayer;
		}
		else if (!hasLineOfSight)
		{
			// 在合适距离但没有视线，尝试绕开障碍物
			moveDirection = GetAvoidanceDirection(directionToPlayer);
		}
		else if (Mathf.Abs(distanceToPlayer - preferredDistance) > distanceTolerance)
		{
			// 调整到理想距离
			if (distanceToPlayer < preferredDistance)
			{
				moveDirection = -directionToPlayer * 0.5f; // 缓慢后退
			}
			else
			{
				moveDirection = directionToPlayer * 0.5f; // 缓慢靠近
			}
		}

		// 检测并避开前方障碍物
		if (moveDirection != Vector2.zero)
		{
			moveDirection = ApplyObstacleAvoidance(moveDirection);
			
			// 检测目标位置是否可达（防止穿墙）
			Vector2 targetPos = currentPos + moveDirection.normalized * actualMoveSpeed * Time.deltaTime;
			if (!IsPositionBlocked(targetPos))
			{
				transform.Translate(moveDirection.normalized * actualMoveSpeed * Time.deltaTime);
			}
		}
	}

	/// <summary>
	/// 检测目标位置是否被阻挡
	/// </summary>
	protected bool IsPositionBlocked(Vector2 targetPos)
	{
		Vector2 currentPos = transform.position;
		Vector2 direction = targetPos - currentPos;
		float distance = direction.magnitude;
		
		// 从当前位置向目标位置发射射线
		RaycastHit2D hit = Physics2D.Raycast(currentPos, direction.normalized, distance + 0.1f, obstacleLayer);
		return hit.collider != null;
	}

	/// <summary>
	/// 检查是否有直接视线到玩家（无障碍物阻挡）
	/// </summary>
	protected bool CheckLineOfSight()
	{
		if (player == null) return false;

		Vector2 origin = firePoint != null ? firePoint.position : transform.position;
		Vector2 playerPosition = player.transform.position;
		Vector2 direction = playerPosition - origin;
		float distance = direction.magnitude;

		// 临时禁用自己的碰撞体，防止射线检测到自己
		bool wasEnabled = false;
		if (myCollider != null)
		{
			wasEnabled = myCollider.enabled;
			myCollider.enabled = false;
		}

		// 射线检测，检查是否有障碍物（使用所有层，但排除玩家层）
		RaycastHit2D hit = Physics2D.Raycast(origin, direction.normalized, distance, obstacleLayer);
		
		// 恢复碰撞体
		if (myCollider != null)
		{
			myCollider.enabled = wasEnabled;
		}
		
		// 如果没有击中任何障碍物，则有视线
		return hit.collider == null;
	}

	/// <summary>
	/// 获取绕开障碍物的方向
	/// </summary>
	protected Vector2 GetAvoidanceDirection(Vector2 desiredDirection)
	{
		// 尝试左右两侧找到可通行的路径
		float[] angles = { 30f, -30f, 60f, -60f, 90f, -90f, 120f, -120f };
		
		foreach (float angle in angles)
		{
			Vector2 testDirection = RotateVector(desiredDirection, angle);
			Vector2 origin = transform.position;
			
			RaycastHit2D hit = Physics2D.Raycast(origin, testDirection, obstacleCheckDistance, obstacleLayer);
			
			if (hit.collider == null)
			{
				// 这个方向没有障碍物，可以尝试走这个方向
				return testDirection;
			}
		}

		// 如果所有方向都被阻挡，返回原方向
		return desiredDirection;
	}

	/// <summary>
	/// 应用障碍物避让
	/// </summary>
	protected Vector2 ApplyObstacleAvoidance(Vector2 moveDirection)
	{
		Vector2 origin = transform.position;
		
		// 检测前方是否有障碍物
		RaycastHit2D frontHit = Physics2D.Raycast(origin, moveDirection, obstacleCheckDistance, obstacleLayer);
		
		if (frontHit.collider != null)
		{
			// 前方有障碍物，计算避让方向
			Vector2 avoidDir = GetAvoidanceDirection(moveDirection);
			return Vector2.Lerp(moveDirection, avoidDir, avoidanceStrength * Time.deltaTime).normalized;
		}

		return moveDirection;
	}

	/// <summary>
	/// 旋转向量
	/// </summary>
	protected Vector2 RotateVector(Vector2 v, float degrees)
	{
		float radians = degrees * Mathf.Deg2Rad;
		float cos = Mathf.Cos(radians);
		float sin = Mathf.Sin(radians);
		return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
	}

	/// <summary>
	/// 发射子弹
	/// </summary>
	protected virtual void Shoot()
	{
		if (bulletPrefab == null)
		{
			Debug.LogWarning("Monster2: 未设置子弹预制体！");
			return;
		}

		Vector2 spawnPos = firePoint != null ? firePoint.position : transform.position;
		Vector2 direction = ((Vector2)player.transform.position - spawnPos).normalized;

		GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
		
		// 设置子弹旋转朝向目标
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

		// 获取或添加子弹组件
		MonsterBullet bulletComponent = bullet.GetComponent<MonsterBullet>();
		if (bulletComponent == null)
		{
			bulletComponent = bullet.AddComponent<MonsterBullet>();
		}
		
		bulletComponent.Initialize(direction, bulletSpeed, bulletRange, attackPower, obstacleLayer,isAggroToMonsters);
	}

	protected override void OnPlayerCollision(GameObject playerObject)
	{
		// 远程怪物碰撞时也可以造成伤害，但伤害较低
		Debug.Log("Monster2: 与玩家发生碰撞");
		if (playerStats != null)
		{
			playerStats.TakeDamage(attackPower / 2); // 近战伤害减半
		}
	}

	protected override void Attack()
	{
		// 远程攻击通过Shoot方法实现
	}

	// 用于在编辑器中可视化调试
	protected virtual void OnDrawGizmosSelected()
	{
		// 绘制距离圈
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, minDistance);
		
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, preferredDistance);
		
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, maxDistance);
		
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, bulletRange);
	}
}
