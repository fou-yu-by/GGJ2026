using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSkill : Skill
{
    
    [SerializeField] private GameObject FireBallPrefab;

    protected override void UseSkill()
    {
        base.UseSkill();
        GameObject fireBall = Instantiate(FireBallPrefab, transform.position, transform.rotation);
        //在20mi内时发射
        if (CheckNearestTarget() != null)
        {
            fireBall.GetComponent<FireBall>().CacheTarget(CheckNearestTarget(), flySpeed, 
                Mathf.RoundToInt(MainPlayer.Instance.playerStats.damage.GetValue() * damageMultiplier), damageRange);
        }
        else
        {
            if (MainPlayer.Instance.inputMovement != Vector2.zero)
            {
                fireBall.GetComponent<FireBall>().rb.velocity = MainPlayer.Instance.inputMovement * flySpeed;
            }
            else
            {
                fireBall.GetComponent<FireBall>().rb.velocity = Vector3.right * flySpeed;
            }

            fireBall.GetComponent<FireBall>().SetDefaultValue(flySpeed,
                Mathf.RoundToInt(MainPlayer.Instance.playerStats.damage.GetValue() * damageMultiplier));
            Destroy(fireBall, 4f);
        }
    }


}
