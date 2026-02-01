using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSkill : Skill
{
    [SerializeField] private GameObject arrowPrefab;

    protected override void UseSkill()
    {
        base.UseSkill();
        float rotateAngle;
        Vector2 shootDirection;
        
        // 优先寻找场上激活的怪物
        Transform nearestMonster = FindNearestMonster();
        
        if (nearestMonster != null)
        {
            // 朝向最近的怪物发射
            shootDirection = (nearestMonster.position - MainPlayer.Instance.transform.position).normalized;
        }
        else
        {
            // 没有怪物时，使用当前移动方向，如果静止则使用最近的方向
            shootDirection = MainPlayer.Instance.inputMovement != Vector2.zero 
                ? MainPlayer.Instance.inputMovement 
                : MainPlayer.Instance.lastDirection;
        }
        
        // 箭的Sprite默认朝上，所以需要减去90度
        rotateAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg - 90f;
        GameObject arrow = Instantiate(arrowPrefab, MainPlayer.Instance.transform.position, Quaternion.Euler(0, 0, rotateAngle));
        arrow.GetComponent<Arrow>().SetDefaultValues(flySpeed,
            Mathf.RoundToInt(MainPlayer.Instance.playerStats.damage.GetValue() * damageMultiplier), shootDirection);
        
    }
    
    /// <summary>
    /// 寻找场上最近的激活怪物
    /// </summary>
    private Transform FindNearestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        
        Transform nearest = null;
        float minDistance = float.MaxValue;
        Vector3 playerPos = MainPlayer.Instance.transform.position;
        
        foreach (GameObject monster in monsters)
        {
            if (monster.activeInHierarchy)
            {
                // 检查怪物是否已激活
                Monster monsterComponent = monster.GetComponent<Monster>();
                if (monsterComponent == null || !monsterComponent.IsActivated)
                    continue;
                    
                float distance = Vector3.Distance(playerPos, monster.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = monster.transform;
                }
            }
        }
        
        return nearest;
    }
}
