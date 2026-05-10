using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    private static DataManager instance;
    public static DataManager Instance => instance;

    private  float volumeMusic;
    private  float volumeSound;
    private  bool musicOn;
    private  bool soundOn;
    private bool isDontFirstPlay = false;


    public  float GetVolumeMusic() => volumeMusic;
    public float GetVolumeSound() => volumeSound;
    public bool GetSoundOn() => soundOn;
    public bool GetMusicOn() => musicOn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMusicData();
            LoadSoundData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMusicData()
    {
        volumeMusic = PlayerPrefs.GetFloat("VolumeMusic", 0.5f);      
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
    }

    public void SaveMusicValueData( float value)
    {
        PlayerPrefs.SetFloat("VolumeMusic", value);
        volumeMusic = PlayerPrefs.GetFloat("VolumeMusic", 0.5f);
        PlayerPrefs.Save();
    }

    public void SaveMusicStateData(bool isOn)
    {
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        PlayerPrefs.Save();
    }

    public void LoadSoundData()
    {
        volumeSound = PlayerPrefs.GetFloat("VolumeSound", 0.5f);
        soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
    }

    public void SaveSoundValueData(float value)
    {
        PlayerPrefs.SetFloat("VolumeSound", value);
        volumeSound = PlayerPrefs.GetFloat("VolumeSound", 0.5f);
        PlayerPrefs.Save();
    }

    public void SaveSoundStateData(bool isOn)
    {
        PlayerPrefs.SetInt("SoundOn", isOn ? 1 : 0);
        soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        PlayerPrefs.Save();
    }

    public void SetIsDontFirstPlay(bool isOn)
    {
        PlayerPrefs.SetInt("IsFirstPlay", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool GetIsDontFirstPlay()
    {
        int isTrue = PlayerPrefs.GetInt("IsFirstPlay", 0);
        isDontFirstPlay = isTrue == 1;
        return isDontFirstPlay;
    }
}
