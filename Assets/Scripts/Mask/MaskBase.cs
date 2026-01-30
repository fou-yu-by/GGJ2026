using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mask", menuName = "Mask/ Specific Mask")]
public class MaskBase : ScriptableObject
{
   public MaskType MaskType;
   
   public string MaskName;
   public Sprite MaskIcon;
   
   
   
   public int modifierValue;
   
    
}
