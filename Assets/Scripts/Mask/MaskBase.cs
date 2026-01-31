using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaskBase : ScriptableObject
{
   public MaskType MaskType;
   public SkillType SkillType;
   
   public string MaskName;
   public Sprite MaskIcon;


   public List<MaskModifier>  MaskModifiers;
   
   

}

[System.Serializable]
public class MaskModifier
{
   public string modifierName;
   public int modifierValue;
   public float modifierDuration;
}