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
        if (MainPlayer.Instance.inputMovement != Vector2.zero)
        {
            rotateAngle = Mathf.Atan2(MainPlayer.Instance.inputMovement.y, MainPlayer.Instance.inputMovement.x) *
                                Mathf.Rad2Deg;
        GameObject arrow = Instantiate(arrowPrefab, MainPlayer.Instance.transform.position, Quaternion.Euler(0, 0, rotateAngle));
        arrow.GetComponent<Arrow>().SetDefaultValues(flySpeed,
            Mathf.RoundToInt(MainPlayer.Instance.playerStats.damage.GetValue() * damageMultiplier), MainPlayer.Instance.inputMovement);
        }
        else
        {
            rotateAngle = 0f;
            GameObject arrow = Instantiate(arrowPrefab, MainPlayer.Instance.transform.position, Quaternion.Euler(0, 0, rotateAngle));
            arrow.GetComponent<Arrow>().SetDefaultValues(flySpeed,
                Mathf.RoundToInt(MainPlayer.Instance.playerStats.damage.GetValue() * damageMultiplier), Vector2.right);
        }
        
    }
}
