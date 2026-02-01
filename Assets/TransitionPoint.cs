using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TransitionManager.Instance.TransitionToScene(TransitionManager.Instance.currentScene, "Level2");
        }
    }
}
