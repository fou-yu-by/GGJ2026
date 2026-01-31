using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   private Transform player;
   
   private void Start()
   {
      player = GameObject.FindGameObjectWithTag("Player").transform;
      transform.SetParent(player.transform);
   }
}
