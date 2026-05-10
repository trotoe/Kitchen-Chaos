using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainmenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button endButton;

    private void Start()
    {
        ////²âÊÔÓÃ
        //DataManager.Instance.SetIsDontFirstPlay(false);

        startButton.onClick.AddListener(()=>
        {
            //LoadingManager.Instacne.Load(E_SceneState.GameScene);
            LoadingManager.Load(E_SceneState.GameScene);
        });

        settingButton.onClick.AddListener(() =>
        {
            SettingUI.Instance.Show();
        });

        endButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    
}
