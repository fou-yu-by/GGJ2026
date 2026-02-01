using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : Singleton<SkillManager>
{
    public FireBallSkill FireBallSkill;
    public ArrowSkill ArrowSkill;
    public JoyHaloSkill JoyHaloSkill;
    public MadHaloSkill MadHaloSkill;
    
    
    private MaskBase skillMask;
    
    public Image skillImage;
    public Slider CDUI;
    
    public bool canUseFireBall;
    public bool canUseArrow;
    public bool canUseJoyHalo;
    public bool canUseMadHalo;

    
    protected override void Awake()
    {
        base.Awake();
        FireBallSkill = GetComponent<FireBallSkill>();
        ArrowSkill = GetComponent<ArrowSkill>();
        JoyHaloSkill = GetComponent<JoyHaloSkill>();
        MadHaloSkill = GetComponent<MadHaloSkill>();
    }

    private void Start()
    {
        //获取当前装备的面具
        skillMask = EquipManager.Instance.Mask;
        
    }

    private void Update()
    {
        CheckCurrentSkill();
        ShowSkillCD();
        
    }

    private void OnEnable()
    {
        //拾起面具时调用的事件
        EventManager.Instance.AddListener("AfterPickUpTheMask", OnAfterPickUpTheMask);
    }

    private void OnAfterPickUpTheMask(object sender, EventArgs e)
    {
        skillMask = EquipManager.Instance.Mask;
        //拾取面具时检查面具是否携带技能
        CheckCurrentSkill();
        //改变技能UI
        UpdateSkillUI();
    }

    private void UpdateSkillUI()
    {

        // if (canUseFireBall)
        // {
        //     this.skillImage.sprite = FireBallSkill.skillImage;
        //     this.skillImage.GetComponentInChildren<Text>().text = SkillType.FireBall.ToString();
        //     
        // }
        // else if (canUseArrow)
        // {
        //     this.skillImage.sprite = ArrowSkill.skillImage;
        //     this.skillImage.GetComponentInChildren<Text>().text = SkillType.Shoot.ToString();
        // }
        if (canUseJoyHalo)
        {
            this.skillImage.sprite = JoyHaloSkill.skillImage;
            this.skillImage.GetComponentInChildren<Text>().text = SkillType.HaloOfJoy.ToString();
        }
        else if (canUseMadHalo)
        {
            this.skillImage.sprite = MadHaloSkill.skillImage;
            this.skillImage.GetComponentInChildren<Text>().text = SkillType.HaloOfAnger.ToString();
        }
        else //当前无任何可用技能时
        {
            this.skillImage.sprite = null;
            this.skillImage.GetComponentInChildren<Text>().text = "";
        }
    }

    private void CheckCurrentSkill()
    {
        //检测火球术
        foreach (var unlockedMask in  FireBallSkill.unlockedMasks)
        {
            if (unlockedMask == skillMask)
            {
                canUseFireBall = true;
                return;
            }
            
        }
        canUseFireBall = false;
        foreach (var unlockedMask in ArrowSkill.unlockedMasks)
        {
            if (unlockedMask == skillMask)
            {
                canUseArrow = true;
                return;
            }
        }
        canUseArrow = false;

        foreach (var unlockedMask in JoyHaloSkill.unlockedMasks)
        {
            if (unlockedMask == skillMask)
            {
                canUseJoyHalo = true;
                return;
            }
        }
        canUseJoyHalo = false;

        foreach (var unlockedMask in MadHaloSkill.unlockedMasks)
        {
            if (unlockedMask == skillMask)
            {
                canUseMadHalo =  true;
                return;
            }
        }
        canUseMadHalo =  false;
    }


    private void ShowSkillCD()
    {
        // if (canUseFireBall)
        // {
        //     CDUI.value = Mathf.Clamp(FireBallSkill.GetCurrentCDCondition() , 0,1);
        // }
        // else if (canUseArrow)
        // {
        //     CDUI.value = Mathf.Clamp(ArrowSkill.GetCurrentCDCondition() , 0,1);
        //     
        // }
        if (canUseJoyHalo)
        {
            CDUI.value = Mathf.Clamp(JoyHaloSkill.GetCurrentCDCondition() , 0,1);
        }
        else if (canUseMadHalo)
        {
            CDUI.value = Mathf.Clamp(MadHaloSkill.GetCurrentCDCondition() , 0,1);
        }
        else
        {
            CDUI.value = 0;
        }
    }
    
    
    
}
