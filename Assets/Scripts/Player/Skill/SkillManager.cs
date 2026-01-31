using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : Singleton<SkillManager>
{
    public FireBallSkill FireBallSkill;
    public ArrowSkill ArrowSkill;
    
    private MaskBase skillMask;
    
    public Image skillImage;
    public Slider CDUI;
    
    public bool canUseFireBall;
    public bool canUseArrow;

    
    protected override void Awake()
    {
        base.Awake();
        FireBallSkill = GetComponent<FireBallSkill>();
        ArrowSkill = GetComponent<ArrowSkill>();
    }

    private void Start()
    {
        //获取当前装备的面具
        skillMask = EquipManager.Instance.Mask;
        
    }

    private void Update()
    {
        CheckCurrentSkill();
    }

    private void OnEnable()
    {
        //拾起面具时调用的事件
        EventManager.Instance.AddListener("AfterPickUpTheMask", OnAfterPickUpTheMask);
    }

    private void OnAfterPickUpTheMask(object sender, EventArgs e)
    {
        //拾取面具时检查面具是否携带技能
        CheckCurrentSkill();
        //TODO:改变技能UI
        UpdateSkillUI();
    }

    private void UpdateSkillUI()
    {

        if (canUseFireBall)
        {
            this.skillImage.sprite = FireBallSkill.skillImage;
            this.skillImage.GetComponentInChildren<Text>().text = SkillType.FireBall.ToString();
            
        }
        else if (canUseArrow)
        {
            this.skillImage.sprite = ArrowSkill.skillImage;
            this.skillImage.GetComponentInChildren<Text>().text = SkillType.Shoot.ToString();
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
            if (unlockedMask == EquipManager.Instance.Mask)
            {
                canUseFireBall = true;
                return;
            }
            
        }
        canUseFireBall = false;
        foreach (var unlockedMask in ArrowSkill.unlockedMasks)
        {
            if (unlockedMask == EquipManager.Instance.Mask)
            {
                canUseArrow = true;
                return;
            }
        }
        canUseArrow = false;
    }

    
}
