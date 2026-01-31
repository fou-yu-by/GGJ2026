using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPrefab : MonoBehaviour
{
    public MaskBase mask;
    public LayerMask playerMask;
    
    private void OnEnable()
    {
        EventManager.Instance.AddListener("AfterPickUpTheMask", OnAfterPickUpTheMask);
    }

    private void OnAfterPickUpTheMask(object sender, EventArgs e)
    {
        Destroy(gameObject);
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
            hit.GetComponent<MainPlayer>().PickUpTheMask(mask);
        }
        
    }
}
