using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPrefab : MonoBehaviour
{
    public MaskBase mask;
    public LayerMask playerMask;
    

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = mask.MaskIcon;
    }


    private void Update()
    {
        CheckPlayer();
    }

    private void CheckPlayer()
    {
        var hit =  Physics2D.OverlapCircle(transform.position, 1.0f, playerMask);
        if (hit != null && hit.GetComponent<MainPlayer>() != null)
        {
            hit.GetComponent<MainPlayer>().PickUpTheMask(mask,this.gameObject);
            //TODO:播放拾取音效
        }
        
    }
    
}
