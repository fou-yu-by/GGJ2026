using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GameOverCoroutine());
    }


    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(10);
        while (true)
        {
            if (GameObject.FindGameObjectWithTag("Monster") == null) ;
            {
                yield return new WaitForSeconds(1);
                TransitionManager.Instance.TransitionToScene(TransitionManager.Instance.currentScene, "Thanks");
                
            }
            yield return null;
        }
        
    }
}
