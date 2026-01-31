using System.Collections.Generic;
using UnityEngine;

public class MonsterActivator : MonoBehaviour
{
    [Header("激活设置")]
    [SerializeField] private float activationDistance = 5f;
    [SerializeField] private List<GameObject> monsterObjects = new List<GameObject>();
    
    private Transform player;
    private bool hasActivated = false;

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
        if (hasActivated || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= activationDistance)
        {
            ActivateAllMonsters();
        }
    }

    private void ActivateAllMonsters()
    {
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
}
