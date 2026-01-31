using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float coolDownTimer;
    public MaskBase[] unlockedMasks;
    public LayerMask targetLayer;
    
    [Header("技能参数")]
    public float damageMultiplier;
    public float checkTargetDistance;
    public float damageRange;
    
    public Sprite skillImage;
    
    protected virtual void Start()
    {
        
        
    }

    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }
    
    public bool CanUseSkill()
    {
        if (coolDownTimer < 0)
        {
            UseSkill();
            coolDownTimer = coolDown;
            return true;
        }
        Debug.Log("colding");
        return false;
    }
    
    protected virtual void UseSkill()
    {
        
    }
    
    
    
}
