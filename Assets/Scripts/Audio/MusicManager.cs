using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance => instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    { 
        audioSource = GetComponent<AudioSource>();
        audioSource.mute = !DataManager.Instance.GetMusicOn();
        audioSource.volume = DataManager.Instance.GetVolumeMusic();
    }

    public void PlayMusic(bool isOn)
    {
        audioSource.mute = !isOn;
        
    }

    public void SetMusicVolume(float v)
    {
        audioSource.volume = v;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
