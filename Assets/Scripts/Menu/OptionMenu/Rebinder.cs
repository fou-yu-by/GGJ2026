using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rebinder : MonoBehaviour
{
   [SerializeField] private InputActionReference ActionRef;

   private string BINDING_KEY;
   
   //需要改键的按钮
   public Button rebindingButton;

   private void Start()
   {
      BINDING_KEY = ActionRef.action.name + "Rebinding";
      LoadBindings();
      UpdateButtonText();
      rebindingButton.onClick.AddListener(()=>StartRebinding());
   }

   private void UpdateButtonText()
   {
      if (rebindingButton != null)
      {
         if (rebindingButton.GetComponentInChildren<RebindButton>().buttonText != null)
         {
            var buttonTextComponent = rebindingButton.GetComponentInChildren<RebindButton>().buttonText;
            string bindingPath = ActionRef.action.bindings[0].effectivePath;
            if (!string.IsNullOrEmpty(bindingPath))
            {
               buttonTextComponent.text = InputControlPath.ToHumanReadableString(bindingPath,
                  InputControlPath.HumanReadableStringOptions.OmitDevice
                  );
            }else{ Debug.Log("未绑定");}
         }
      }
   }

   private void StartRebinding()
   {
       ActionRef.action.Disable();
       var rebindOperation = ActionRef.action.PerformInteractiveRebinding()
          .WithControlsExcluding("Mouse")
          .OnMatchWaitForAnother(0.1f)
          .OnComplete(operation =>
          {
             //保存按钮绑定为json
             SaveBindings();
             //更新按钮文本
             var name = operation.action.bindings[0].effectivePath;
             rebindingButton.GetComponentInChildren<RebindButton>().buttonText.text =
                InputControlPath.ToHumanReadableString(name, InputControlPath.HumanReadableStringOptions.OmitDevice);
             ActionRef.action.Enable();
             operation.Dispose();
          })
          .Start();
   }


   #region  存取改键数据

    
   private void SaveBindings()
   {
      string bindingJson = ActionRef.action.actionMap.SaveBindingOverridesAsJson();
      PlayerPrefs.SetString(BINDING_KEY, bindingJson);
      PlayerPrefs.Save();
   }

   private void LoadBindings()
   {
      if (PlayerPrefs.HasKey(BINDING_KEY))
      {
         string bindingJson = PlayerPrefs.GetString(BINDING_KEY);
         ActionRef.action.actionMap.LoadBindingOverridesFromJson(bindingJson);
            
      }
   }

   #endregion

}
