using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum E_SceneState
{
    MainMenuScene,
    GameScene,
    LoadingScene,
}

public static class LoadingManager
{
    private static E_SceneState state;
    public static E_SceneState State => state;

    public static void Load(E_SceneState state)
    {
        LoadingManager.state = state;
        Debug.Log($"LoadingManager.Load called: {state}");
        SceneManager.LoadScene(E_SceneState.LoadingScene.ToString());
    }

    //public static void LoadCallBalk()
    //{
    //    SceneManager.LoadScene(state.ToString());
    //}
}
