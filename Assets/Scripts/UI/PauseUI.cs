using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button btnResume;
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnMenu;

    private void Start()
    {
        Hide();
        GameManager.Instance.OnGameToggle += GmaeManager_OnGameToggle;
        btnResume.onClick.AddListener(()=>
        {
            GameManager.Instance.ToggleGame();
        });
        btnSettings.onClick.AddListener(()=>
        {
            SettingUI.Instance.Show(this.gameObject);
        });
        btnMenu.onClick.AddListener(()=>
        {
            Time.timeScale = 1f;
            LoadingManager.Load(E_SceneState.MainMenuScene);
        });
    }

    private void GmaeManager_OnGameToggle(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePaused)
        {
            //ÔÝÍ£Ê±½ûÓÃ¼üÅÌ¼àÌý
            GameInput.Instance.DisableInput();
            Show();
        }
        else
        {
            GameInput.Instance.EnableInput();
            Hide();
        }
    }

    private void Show()
    { 
        this.gameObject.SetActive(true);
    }

    private void Hide()
    { 
        this.gameObject.SetActive(false);
    }
}
