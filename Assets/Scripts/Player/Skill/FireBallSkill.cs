using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSkill : Skill
{
    
    [SerializeField] private GameObject FireBallPrefab;

    protected override void UseSkill()
    {
        base.UseSkill();
        
        float rotateAngle;
        Vector2 shootDirection;
        Transform target = CheckNearestTarget();
        
        if (target != null)
        {
            // 朝向最近的激活怪物发射（追踪模式）
            shootDirection = (target.position - MainPlayer.Instance.transform.position).normalized;
            
            // 火球的Sprite默认朝下，所以需要加90度
            rotateAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + 90f;
            GameObject fireBall = Instantiate(FireBallPrefab, MainPlayer.Instance.transform.position, Quaternion.Euler(0f, 0f, rotateAngle));
            fireBall.GetComponent<FireBall>().CacheTarget(target, flySpeed, 
                Mathf.RoundToInt(MainPlayer.Instance.playerStats.damage.GetValue() * damageMultiplier), damageRange);
        }
        else
        {
            // 没有怪物时，使用当前移动方向，如果静止则使用最近的方向
            shootDirection = MainPlayer.Instance.inputMovement != Vector2.zero 
                ? MainPlayer.Instance.inputMovement 
                : MainPlayer.Instance.lastDirection;
            
            // 火球的Sprite默认朝下，所以需要加90度
            rotateAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + 90f;
            GameObject fireBall = Instantiate(FireBallPrefab, MainPlayer.Instance.transform.position, Quaternion.Euler(0f, 0f, rotateAngle));
            fireBall.GetComponent<FireBall>().rb.velocity = shootDirection.normalized * flySpeed;
            fireBall.GetComponent<FireBall>().SetDefaultValue(flySpeed,
                Mathf.RoundToInt(MainPlayer.Instance.playerStats.damage.GetValue() * damageMultiplier));
        }
    }



}
