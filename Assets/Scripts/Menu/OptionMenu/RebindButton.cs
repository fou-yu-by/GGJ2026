using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Image buttonBg;
    public Text buttonText;
    private void Start()
    {
        buttonBg.color = Color.gray;
        
    }

    public void OnButtonClick()
    {
        buttonBg.color = Color.red;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonBg.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonBg.color = Color.gray;
    }
}
