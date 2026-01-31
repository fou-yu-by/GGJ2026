using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSkill : Skill
{
    [SerializeField] private float flySpeed;
    [SerializeField] private GameObject FireBallPrefab;

    protected override void UseSkill()
    {
        base.UseSkill();
        GameObject fireBall = Instantiate(FireBallPrefab, transform.position, transform.rotation);
        //在20mi内时发射
        if (CheckNearestTarget() != null)
        {
            fireBall.GetComponent<FireBall>().CacheTarget(CheckNearestTarget(), flySpeed, 
                Mathf.RoundToInt(MainPlayer.Instance.playerStats.damage.GetValue() * damageMultiplier));
        }
    }

    private Transform CheckNearestTarget()
    {
        Transform nearestTarget = null;
        var hits = Physics2D.OverlapCircleAll(MainPlayer.Instance.transform.position, checkTargetDistance,targetLayer);
        float minDistance = Mathf.Infinity;
        foreach (var hitInfo in hits)
        {
            if (Vector2.Distance(MainPlayer.Instance.transform.position, hitInfo.transform.position) <= minDistance)
            {
                minDistance = Vector2.Distance(MainPlayer.Instance.transform.position, hitInfo.transform.position);
                nearestTarget = hitInfo.transform;
            }
        }
        return nearestTarget;
    }
}
