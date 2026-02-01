using System.Collections.Generic;
using UnityEngine;

public class MonsterActivator : MonoBehaviour
{
    [Header("激活设置")]
    [SerializeField] private float activationDistance = 5f;
    [SerializeField] private List<GameObject> monsterObjects = new List<GameObject>();
    [SerializeField] private Vector2 offset = Vector2.zero;
    [Header("激活其他点")]
    [SerializeField] private List<GameObject> otherActivationPoints = new List<GameObject>();
    [SerializeField] private float otherPointActivationDelay = 1f;
    private Transform player;
    private bool hasActivated = false;
    private bool isAllDead=false;
    private void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("MonsterActivator: 未找到名为 'Player' 的对象。");
        }
    }

    private void Update()
    {
        if (player == null) return;
        if (hasActivated)
        {
            if (!isAllDead)
            {
                isAllDead = true;
                foreach (var obj in monsterObjects)
                {
                    if (obj != null)
                    {
                        Monster monster = obj.GetComponent<Monster>();
                        if (monster != null && monster.gameObject.activeInHierarchy && monster.CurrentHp > 0)
                        {
                            isAllDead = false;
                            break;
                        }
                    }
                }
                if (isAllDead)
                {
                    Debug.Log("MonsterActivator: 所有怪物已被击败！");
                    // 怪物全部死亡时，激活其他点
                    if (otherActivationPoints != null && otherActivationPoints.Count > 0)
                    {
                        Invoke(nameof(ActiveOtherPoints), otherPointActivationDelay);
                    }
                }
            }
            return;
        }
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= activationDistance)
        {
            ActivateAllMonsters();
        }
    }

    private void ActivateAllMonsters()
    {
        transform.position += (Vector3)offset;
        hasActivated = true;
        int activatedCount = 0;
        foreach (var obj in monsterObjects)
        {
            if (obj != null)
            {
                Monster monster = obj.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.Activate();
                    activatedCount++;
                }
                else
                {
                    Debug.LogWarning($"MonsterActivator: 对象 {obj.name} 上未找到 Monster 组件");
                }
            }
        }
        Debug.Log($"MonsterActivator: 已激活 {activatedCount} 个怪物");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
    public void BeActivated()
    {
        ActivateAllMonsters();
    }
    private void ActiveOtherPoints()
    {
        if (otherActivationPoints == null || otherActivationPoints.Count == 0) return;
        
        foreach (var point in otherActivationPoints)
        {
            if (point != null)
            {
                MonsterActivator activator = point.GetComponent<MonsterActivator>();
                if (activator != null)
                {
                    activator.BeActivated();
                }
            }
        }
    }
}
