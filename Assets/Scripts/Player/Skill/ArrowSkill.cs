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
        }
        else
        {
            rotateAngle = 0f;
        }
        
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0, 0, rotateAngle));
        Destroy(arrow, 4f);
    }
}
