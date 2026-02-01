using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReset : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TransitionManager.Instance.TransitionToScene(TransitionManager.Instance.currentScene,TransitionManager.Instance.startScene);
        }
    }
    
    
}
