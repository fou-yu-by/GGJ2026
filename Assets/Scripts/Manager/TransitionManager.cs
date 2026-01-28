using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
    public string startScene;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        TransitionToScene(string.Empty, startScene);
    }

    public void TransitionToScene(string from, string to)
    {
        StartCoroutine(TransitionCoroutine(from, to));
    }
    
    IEnumerator TransitionCoroutine(string from, string to)
    {
        if (from != string.Empty)
        {
            yield return SceneManager.UnloadSceneAsync(from);
        }

        yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
    }
}
