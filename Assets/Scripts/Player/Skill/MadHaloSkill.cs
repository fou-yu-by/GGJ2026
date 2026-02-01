using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadHaloSkill : Skill
{
    private List<IGetAOEEffect> takeAOEMonsters =  new List<IGetAOEEffect>();
   
    protected override void UseSkill()
    {
        base.UseSkill();
        var hits = Physics2D.OverlapCircleAll(MainPlayer.Instance.transform.position, float.PositiveInfinity,
            targetLayer);
        foreach (var hitInfo in hits)
        {
            if (hitInfo.CompareTag("Monster") && hitInfo.isActiveAndEnabled)
            {
                takeAOEMonsters.Add(hitInfo.GetComponent<IGetAOEEffect>());
            }
        }

        foreach (var monster in takeAOEMonsters)
        {
            monster.GetAOEEffect(2,5);
        }

    }
}
