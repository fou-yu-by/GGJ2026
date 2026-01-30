using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MoveCompositeRebinder : MonoBehaviour
{
    public Button rebindUpButton;
    public Button rebindDownButton;
    public Button rebindLeftButton;
    public Button rebindRightButton;
    
    [Header("Input Action Reference")]
    public InputActionReference moveActionReference;
    
    private InputAction moveAction;
    
    
    private const string BINDING_KEY_MOVE = "MoveCompositeRebinding";

    private void Start()
    {
        moveAction = moveActionReference.action;

        if (!moveAction.enabled)
        {
            moveAction.Enable();
        }
        
        //导入改键设置
        LoadBindings();
        UpdateButtonText();
        
        rebindUpButton.onClick.AddListener(() => StartRebinding("Up"));
        rebindDownButton.onClick.AddListener(() => StartRebinding("Down"));
        rebindLeftButton.onClick.AddListener(() => StartRebinding("Left"));
        rebindRightButton.onClick.AddListener(() => StartRebinding("Right"));
    }

    private void StartRebinding(string name)
    {
        int bindingIndex = -1;
        switch (name)
        {
            case "Up":
                bindingIndex = moveAction.bindings.IndexOf(binding => binding.name == "Up" || binding.name == "up");
                break;
            case "Down":
                bindingIndex = moveAction.bindings.IndexOf(binding => binding.name == "Down" || binding.name == "down");
                break;
            case "Left":
                bindingIndex = moveAction.bindings.IndexOf(binding => binding.name == "Left" || binding.name == "left");
                break;
            case "Right":
                bindingIndex = moveAction.bindings.IndexOf(binding => binding.name == "Right" || binding.name == "right");
                break;
        }
        
        if (bindingIndex < 0)
        {
            return;
        }
        //禁用Action Map 以改键
        moveAction.Disable();
        var rebindOperation = moveAction.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                //保存按钮绑定为json
                SaveBindings();
                //更新按钮文本
                UpdateButtonText();
                moveAction.Enable();
                operation.Dispose();
            })
            .Start();

    }

    private void UpdateButtonText()
    {
        rebindUpButton.GetComponent<RebindButton>().buttonText.text = GetBindingString("Up");
        rebindDownButton.GetComponent<RebindButton>().buttonText.text = GetBindingString("Down");
        rebindLeftButton.GetComponent<RebindButton>().buttonText.text = GetBindingString("Left");
        rebindRightButton.GetComponent<RebindButton>().buttonText.text = GetBindingString("Right");
    }
    
    
    
    
    
    //获取按键绑定的字符串
    private string GetBindingString(string _bindName)
    {
        foreach (var binding in moveAction.bindings)
        {
            if (binding.name.Equals(_bindName, StringComparison.InvariantCultureIgnoreCase))
            {
                return binding.ToDisplayString();
            }
        }
        return "";
    }
    
    
    
    #region  存取改键数据

    
    private void SaveBindings()
    {
        string bindingJson = moveAction.actionMap.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(BINDING_KEY_MOVE, bindingJson);
        PlayerPrefs.Save();
    }

    private void LoadBindings()
    {
        if (PlayerPrefs.HasKey(BINDING_KEY_MOVE))
        {
            string bindingJson = PlayerPrefs.GetString(BINDING_KEY_MOVE);
            moveAction.actionMap.LoadBindingOverridesFromJson(bindingJson);
            
        }
    }

    #endregion
    
}
