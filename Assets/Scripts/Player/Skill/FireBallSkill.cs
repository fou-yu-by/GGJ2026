using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSkill : Skill
{
    
    [SerializeField] private GameObject FireBallPrefab;

    protected override void UseSkill()
    {
        base.UseSkill();
        //在20mi内时发射
        if (CheckNearestTarget() != null)
        {
            Vector3 direction = (CheckNearestTarget().position - MainPlayer.Instance.transform.position).normalized;
            float rotateAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject fireBall = Instantiate(FireBallPrefab, MainPlayer.Instance.transform.position, Quaternion.Euler(0f, 0f, rotateAngle));
            fireBall.GetComponent<FireBall>().CacheTarget(CheckNearestTarget(), flySpeed, 
                Mathf.RoundToInt(MainPlayer.Instance.playerStats.damage.GetValue() * damageMultiplier), damageRange);
        }
        else
        {
            
            if (MainPlayer.Instance.inputMovement != Vector2.zero)
            {
                float angle = Mathf.Atan2(MainPlayer.Instance.inputMovement.y, MainPlayer.Instance.inputMovement.x) * Mathf.Rad2Deg;
                GameObject fireBall = Instantiate(FireBallPrefab, MainPlayer.Instance.transform.position, Quaternion.Euler(0f, 0f, angle));
                fireBall.GetComponent<FireBall>().rb.velocity = MainPlayer.Instance.inputMovement * flySpeed;
                fireBall.GetComponent<FireBall>().SetDefaultValue(flySpeed,
                    Mathf.RoundToInt(MainPlayer.Instance.playerStats.damage.GetValue() * damageMultiplier));
            }
            else
            {
                GameObject fireBall = Instantiate(FireBallPrefab, MainPlayer.Instance.transform.position, Quaternion.Euler(0f, 0f, 90f));
                fireBall.GetComponent<FireBall>().rb.velocity = Vector3.right * flySpeed;
                fireBall.GetComponent<FireBall>().SetDefaultValue(flySpeed,
                    Mathf.RoundToInt(MainPlayer.Instance.playerStats.damage.GetValue() * damageMultiplier));
                
            }

            
        }
    }



}
