using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//人物基础数据，包含buff数值叠加
public class Stats
{
    [SerializeField] private int baseValue;
    //buff
    public List<int> modifiers;
    
    
    //经过buff叠加后总的数值
    public int GetValue()
    {
        int finalValue = baseValue;
        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }


    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }
    
    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
