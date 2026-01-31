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
		Vector2 directionToNextPoint = (pathPoints[currentPathIndex] - transform.position).normalized;
		transform.Translate(directionToNextPoint * actualMoveSpeed * Time.deltaTime);
		// Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
		// transform.Translate(directionToPlayer * actualMoveSpeed * Time.deltaTime);
	}
	private void AutoPath()
	{
		if(pathUpdateTimer <= 0f)
		{
			UpdatePath();
			pathUpdateTimer = pathUpdateInterval;
		}
		else
		{
			pathUpdateTimer -= Time.deltaTime;
		}
		if(pathPoints == null || pathPoints.Count <= 0|| currentPathIndex >= pathPoints.Count)
		{
			UpdatePath();
		}
		else if(Vector2.Distance(transform.position,pathPoints[currentPathIndex]) <= 0.1f)
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
		
		seeker.StartPath(transform.position, player.transform.position,Path=> {
			pathPoints = new List<Vector3>(Path.vectorPath);
			currentPathIndex = 0;
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
