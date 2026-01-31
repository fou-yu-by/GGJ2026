using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipManager : Singleton<EquipManager>
{
    public Image professionSlot;
    public Image emotionSlot;
    
    public GameObject maskPrefab;
    public MaskBase Mask;
    
    //存储当前装备的面具信息（面具类型：面具）
    public Dictionary<MaskType, MaskBase> equipmentDict = new Dictionary<MaskType, MaskBase>();
    
    //更改装配图标及名称
    public void UpdateSlotUI()
    {
        MaskBase oldMask;
        //更新装备信息存储字典
        if (equipmentDict.ContainsKey(Mask.MaskType))
        {
            //获取上一个装备的面具以便实现掉落逻辑
            oldMask = equipmentDict[Mask.MaskType];
            equipmentDict[Mask.MaskType] = Mask;
        }
        else
        {
            oldMask = null;
            equipmentDict.Add(Mask.MaskType, Mask);
        }
        //更新UI
        professionSlot.sprite = ((ProfessionMask)Mask).MaskIcon;
        if (Mask.MaskType == MaskType.Professtion)
        {
            professionSlot.GetComponentInChildren<Text>().text = Mask.MaskName;
        }
        else if (Mask.MaskType == MaskType.Emotion)
        {
            emotionSlot.sprite = ((EmotionMask)Mask).MaskIcon;
            emotionSlot.GetComponentInChildren<Text>().text = Mask.MaskName;
        }
        
        //实现上一个面具的掉落
        if (oldMask != null)
        {
           GameObject dropMask = Instantiate(maskPrefab, MainPlayer.Instance.transform.position, Quaternion.identity);
           dropMask.GetComponent<MaskPrefab>().mask = oldMask;
        }
        
    }
    
    //存储为Json
    
    

}
