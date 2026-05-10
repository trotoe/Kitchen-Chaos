using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    private static SettingUI instance;
    public static SettingUI Instance => instance;

    public bool IsOpen { get; private set; }

    [SerializeField] private Button btnBack;
    [SerializeField] private Toggle togMusic;
    [SerializeField] private Slider sldMusic;
    [SerializeField] private Toggle togSound;
    [SerializeField] private Slider sldSound;

    [SerializeField] private Button btnCustomKey;
    [SerializeField] private CustomKeyUI customKeyPanel;

    [SerializeField] private Button btnTutorial;
    [SerializeField] private TutorialUI tutorialPanel;

    private GameObject lastPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Hide();
        customKeyPanel.Hide();
        InitUI();
        btnBack.onClick.AddListener(() =>
        {
            Hide();
            PlayerPrefs.Save();
        });

        btnCustomKey.onClick.AddListener(()=>
        { 
            customKeyPanel.Show();
        });
        
        togMusic.onValueChanged.AddListener((value) =>
        {
            MusicManager.Instance?.PlayMusic(value);
            DataManager.Instance.SaveMusicStateData(value);
        });

        sldMusic.onValueChanged.AddListener((value) =>
        {
            MusicManager.Instance?.SetMusicVolume(value);
            DataManager.Instance.SaveMusicValueData(value);
        });

        togSound.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance?.PlaySound(value);
            DataManager.Instance.SaveSoundStateData(value);
        });

        sldSound.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance?.SetSoundVolume(value);
            DataManager.Instance.SaveSoundValueData(value);
        });

        btnTutorial.onClick.AddListener(() => 
        { 
            tutorialPanel.Show();
        });
    }

    private void InitUI()
    {
        togMusic.isOn = DataManager.Instance.GetMusicOn();
        sldMusic.value = DataManager.Instance.GetVolumeMusic();
        togSound.isOn = DataManager.Instance.GetSoundOn();
        sldSound.value = DataManager.Instance.GetVolumeSound();
    }

    public void Show(GameObject lastPanel = null)
    {
        this.lastPanel = lastPanel;
        IsOpen = true;
        gameObject.SetActive(true);
        lastPanel?.SetActive(false);
    }

    public void Hide()
    {
        IsOpen = false;
        gameObject.SetActive(false);
        lastPanel?.SetActive(true);
    }

    void OnDestroy()
    {
        instance = null;
    }
}
