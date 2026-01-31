using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    private MaskBase skillMask;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //获取当前装备的面具
        skillMask = EquipManager.Instance.Mask;
        
        Debug.Log(SkillType.FireBall.ToString());
    }

    private void OnEnable()
    {
        //拾起面具时调用的事件
        EventManager.Instance.AddListener("AfterPickUpTheMask", OnAfterPickUpTheMask);
    }

    private void OnAfterPickUpTheMask(object sender, EventArgs e)
    {
        //TODO:改变技能UI
        
    }

    private SkillType GetCurrentSkill()
    {
        return skillMask.SkillType;
    }
    
}
