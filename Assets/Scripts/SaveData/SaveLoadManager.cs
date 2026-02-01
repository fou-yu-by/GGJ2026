using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private string savePath;

    private List<ISaveable> _saveablelist = new List<ISaveable>();
    private Dictionary<string, SaveData> _saveDataDic = new Dictionary<string, SaveData>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        savePath = Application.persistentDataPath + "/SaveData/";
        
    }
    

    public void RegisterData(ISaveable saveable)
    {
        _saveablelist.Add(saveable);
    }

    public void Save()
    {
        _saveDataDic.Clear();
        foreach (ISaveable saveable in _saveablelist)
        {
            SaveData saveData = saveable.GenerateSaveData();
            if (_saveDataDic.ContainsKey(saveable.GetType().Name))
            {
                _saveDataDic[saveable.GetType().Name] = saveData;
                
            }
            else
            {
                _saveDataDic.Add(saveable.GetType().Name, saveData);
            }
        }

        var finalPath = savePath + "data.json";
        var jsonData = JsonConvert.SerializeObject(_saveDataDic, Formatting.Indented);
        if (!File.Exists(finalPath))
        {
            Directory.CreateDirectory(savePath);
        }

        File.WriteAllText(finalPath, jsonData);
    }

    public void Load()
    {
        var finalPath = savePath + "data.json";
        if (!File.Exists(finalPath)) return;
        var jsonData = JsonConvert.DeserializeObject<Dictionary<string, SaveData>>(File.ReadAllText(finalPath));
        foreach (ISaveable saveable in _saveablelist)
        {
            saveable.Load(jsonData[saveable.GetType().Name]);
        }
    }
}