using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPrefab : MonoBehaviour
{
    public MaskBase mask;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            other.GetComponent<MainPlayer>().PickUpTheMask(mask);
        }
    }
}
