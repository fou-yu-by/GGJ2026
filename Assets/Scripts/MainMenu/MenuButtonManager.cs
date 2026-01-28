using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonManager : MonoBehaviour,ISaveable
{
    //判断是否为首次游玩(非首次则显示“新游戏”“继续游戏”UI)
    public bool isFirstGame;
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;

    private void Start()
    {
        ISaveable saveable = this;
        saveable.Register();
        
        SaveLoadManager.Instance.Load();
        gameStartButton.gameObject.SetActive(isFirstGame);
        newGameButton.gameObject.SetActive(!isFirstGame);
        continueButton.gameObject.SetActive(!isFirstGame);
        
    }
    
    //TODO:跳转设置画面

    
    
    
    public void LoadGameScene()
    {
        isFirstGame = false;
        SaveLoadManager.Instance.Save();
        //TODO:“开始游戏”加载的场景名称
        //TransitionManager.Instance.TransitionToScene("MainMenu","testScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    #region 存取数据

    public SaveData GenerateSaveData()
    {
        SaveData saveData = new SaveData();
        saveData.isFirstGame = this.isFirstGame;
        return saveData;
    }

    public void Load(SaveData saveData)
    {
        this.isFirstGame = saveData.isFirstGame;
    }
    

    #endregion
}