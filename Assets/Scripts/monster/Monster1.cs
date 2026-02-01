using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Monster1 : Monster
{
	[Header("Movement")]
	[SerializeField] protected float moveSpeed = 5f;
	[SerializeField] protected float movediffrange = 1f;
	private float actualMoveSpeed;
	private Seeker seeker;
	private List<Vector3> pathPoints;
	private int currentPathIndex = 0; 
	[SerializeField] private float pathUpdateInterval = 0.1f;
	private float pathUpdateTimer = 0f;

	protected override void Awake()
	{
		base.Awake();
		seeker = GetComponent<Seeker>();
		actualMoveSpeed = moveSpeed + Random.Range(-movediffrange, movediffrange);
	}

	protected override void Move()
	{
		if (player == null)
		{
			return;
		}
		AutoPath();
		
		// 路径无效时直接朝玩家移动
		if (pathPoints == null || pathPoints.Count == 0 || currentPathIndex >= pathPoints.Count)
		{
			Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
			transform.Translate(directionToPlayer * actualMoveSpeed * Time.deltaTime);
			return;
		}
		
		Vector2 directionToNextPoint = (pathPoints[currentPathIndex] - transform.position).normalized;
		transform.Translate(directionToNextPoint * actualMoveSpeed * Time.deltaTime);
	}
	private void AutoPath()
	{
		if(player==null) return;
		pathUpdateTimer -= Time.deltaTime;
		if(pathUpdateTimer <= 0f)
		{
			UpdatePath();
			pathUpdateTimer = pathUpdateInterval;
		}

		if(pathPoints == null || pathPoints.Count <= 0|| currentPathIndex >= pathPoints.Count)
		{
			UpdatePath();
		}
		else if(currentPathIndex < pathPoints.Count && Vector2.Distance(transform.position,pathPoints[currentPathIndex]) <= 0.1f)
		{
			currentPathIndex++;
			if(currentPathIndex >= pathPoints.Count)
			{
				UpdatePath();
			}
		}

	}
	private void UpdatePath()
	{
		seeker.StartPath(transform.position, player.transform.position, path => {
			if (path.error) return;
			pathPoints = path.vectorPath;
			currentPathIndex = 0;
			// 跳过已经路过或太近的点，避免回头
			while (currentPathIndex < pathPoints.Count - 1 && 
			       Vector2.Distance(transform.position, pathPoints[currentPathIndex]) < 3f)
			{
				currentPathIndex++;
			}
		});
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
