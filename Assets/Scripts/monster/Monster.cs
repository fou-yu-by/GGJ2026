using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class Monster : MonoBehaviour,IGetAOEEffect
{
	[Header("Stats")]
	[SerializeField] protected int maxHp = 100;
	[SerializeField] protected int attackPower = 10;
	[SerializeField] protected float collisionCooldown = 1f;
	
	[Header("父物体设置")]
	[SerializeField] protected Transform parentObject;

	protected int currentHp;
	protected GameObject player;
	protected GameObject truePlayer;
	protected PlayerStats playerStats;
	protected float collisionCooldownTimer = 0f;
	protected bool isActivated = false;

	// 眩晕控制
	protected bool isDizzy = false;
	protected Coroutine dizzyCoroutine;
	
	// 互相攻击控制
	protected bool isAggroToMonsters = false;
	protected Coroutine aggroCoroutine;
	[SerializeField] protected float monsterAttackInterval = 1f; // 攻击其他怪物的间隔
	protected float monsterAttackTimer = 0f;
	
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
	public bool IsActivated => isActivated;
	public virtual void Activate()
	{
		isActivated = true;
	}

	protected virtual void Update()
	{
		if (parentObject != null)// 父物体用来走路，不参与和player的碰撞
		{
			transform.position = parentObject.position;
		}
		else
		{
			if (!isActivated) return;
			
			// 眩晕状态下不移动
			if (!isDizzy)
			{
				Move();
			}
		}
		
		// 互相攻击逻辑
		if (isAggroToMonsters)
		{
			monsterAttackTimer -= Time.deltaTime;
		}
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
		if (isDizzy) return;
		if (isAggroToMonsters)
		{
			// 互相攻击状态下，检测与其他怪物的碰撞
			if (other.CompareTag("Monster") && monsterAttackTimer <= 0f)
			{
				monsterAttackTimer = monsterAttackInterval;
				Monster otherMonster = other.GetComponent<Monster>();
				if (otherMonster != null)
				{
					Debug.Log($"{gameObject.name}: 攻击怪物 {other.name}，攻击力 = {attackPower}");
					otherMonster.TakeDamage(attackPower);
				}
			}
			return;
		}
		if (other == player && collisionCooldownTimer <= 0f)
		{
			collisionCooldownTimer = collisionCooldown;
			OnPlayerCollision(other);
		}
	}
	
	/// <summary>
	/// 实现AOE效果
	/// effectID: 效果ID，effectDuration: 效果持续时间
	/// ID==1: 眩晕效果
	/// ID==2: 互相攻击(monster之间互相攻击)
	/// </summary>
	public void GetAOEEffect(int effectID, float effectDuration)
	{
		if (effectID == 1)
		{
			ApplyDizzyEffect(effectDuration);
		}
		else if (effectID == 2)
		{
			ApplyAggroEffect(effectDuration);
		}
	}

	/// <summary>
	/// 应用眩晕效果
	/// </summary>
	protected virtual void ApplyDizzyEffect(float duration)
	{
		// 如果已有眩晕协程，先停止
		if (dizzyCoroutine != null)
		{
			StopCoroutine(dizzyCoroutine);
		}
		dizzyCoroutine = StartCoroutine(DizzyCoroutine(duration));
	}

	protected IEnumerator DizzyCoroutine(float duration)
	{
		isDizzy = true;
		Debug.Log($"{gameObject.name}: 进入眩晕状态，持续 {duration} 秒");
		
		// 可选：播放眩晕特效或动画
		OnDizzyStart();
		
		yield return new WaitForSeconds(duration);
		
		isDizzy = false;
		Debug.Log($"{gameObject.name}: 眩晕状态结束");
		
		// 可选：停止眩晕特效或动画
		OnDizzyEnd();
		
		dizzyCoroutine = null;
	}

	/// <summary>
	/// 眩晕开始时的回调（子类可重写以添加特效）
	/// </summary>
	protected virtual void OnDizzyStart() { }

	/// <summary>
	/// 眩晕结束时的回调（子类可重写以移除特效）
	/// </summary>
	protected virtual void OnDizzyEnd() { }

	/// <summary>
	/// 应用互相攻击效果
	/// </summary>
	protected virtual void ApplyAggroEffect(float duration)
	{
		// 如果已有互相攻击协程，先停止
		if (aggroCoroutine != null)
		{
			StopCoroutine(aggroCoroutine);
		}
		aggroCoroutine = StartCoroutine(AggroCoroutine(duration));
	}

	protected IEnumerator AggroCoroutine(float duration)
	{
		isAggroToMonsters = true;
		monsterAttackTimer = 0f;
		Debug.Log($"{gameObject.name}: 进入互相攻击状态，持续 {duration} 秒");
		
		OnAggroStart();
		getWrongPlayer();
		yield return new WaitForSeconds(duration);
		
		isAggroToMonsters = false;
		Debug.Log($"{gameObject.name}: 互相攻击状态结束");
		
		OnAggroEnd();
		
		aggroCoroutine = null;
	}
	
	// 将player赋值为最近的tag为Monster并且isactived的对象
	protected void getWrongPlayer()
	{
		player = null;
		float minDistance = float.MaxValue;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");
		foreach (GameObject enemy in enemies)
		{
			Monster monsterComponent = enemy.GetComponent<Monster>();
			if (monsterComponent != null && monsterComponent.IsActivated)
			{
				float distance = Vector2.Distance(transform.position, enemy.transform.position);
				if (distance < minDistance)
				{
					minDistance = distance;
					player = enemy;
				}
			}
		}
		if(player != null)
		{
			Debug.Log($"{gameObject.name}: 将攻击目标设为 {player.name}");
		}
		else
		{
			player = truePlayer;
			Debug.Log($"{gameObject.name}: 未找到可攻击的怪物目标，保持原有目标");
		}
	}
	/// <summary>
	/// 互相攻击开始时的回调
	/// </summary>
	protected virtual void OnAggroStart() { }

	/// <summary>
	/// 互相攻击结束时的回调
	/// </summary>
	protected virtual void OnAggroEnd() { }

	

	/// <summary>
	/// 获取当前是否处于眩晕状态
	/// </summary>
	public bool IsDizzy => isDizzy;

	/// <summary>
	/// 获取当前是否处于互相攻击状态
	/// </summary>
	public bool IsAggroToMonsters => isAggroToMonsters;
}
