using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtonUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
   private Text buttonText;
   private Color textColor;
   void Start()
   {
      buttonText = GetComponentInChildren<Text>();
      textColor = buttonText.color;
      
   }
   
   //鼠标悬浮高亮效果
   public void OnPointerEnter(PointerEventData eventData)
   {
      buttonText.color = Color.white;
      

   }

   public void OnPointerExit(PointerEventData eventData)
   {
      buttonText.color = textColor;
   }
}
