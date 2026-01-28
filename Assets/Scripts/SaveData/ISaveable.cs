using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    void Register() => SaveLoadManager.Instance.RegisterData(this);
    
    SaveData GenerateSaveData();
    void Load(SaveData saveData);

}
