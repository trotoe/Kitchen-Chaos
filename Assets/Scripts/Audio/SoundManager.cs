using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;
    private float soundVolume = 1f;
    private bool isSoundOn = true;


    private void Awake()
    {
        if (instance == null)
        { 
            instance = this;
        }
    }

    private SoundManager()
    {
        
    }

    [SerializeField] private AudioClipSo audioClipSo;
    [SerializeField] private float soundMultiple;

    private void Start()
    {
        soundVolume = DataManager.Instance.GetVolumeSound();
        isSoundOn = DataManager.Instance.GetSoundOn();
        //订阅事件
        //切菜柜子
        CuttingCounter.OnChoping += CuttingCounter_OnChoping;
        CuttingCounter.OnObjectPickedUp += CuttingCounter_OnObjectPickedUp;
        CuttingCounter.OnObjectDrop += CuttingCounter_OnObjectDrop;
        //菜单
        OrderManager.Instance.OnRecipeSuccessed += OrderManager_OnRecipeSuccessed;
        OrderManager.Instance.OnRecipeFailed += OrderManager_OnDeliveryFailed;
        OrderManager.Instance.OnRecipeOverTime += Instance_OnRecipeOverTime;
        //基础柜子拿放物体
        BaseCounter.OnObjectPickedUp += BaseCounter_OnObjectPickedUp;
        BaseCounter.OnObjectDrop += BaseCounter_OnObjectDrop;
        //火炉柜子
        StoveCounter.OnObjectPickedUp += StoveCounter_OnObjectPickedUp;
        StoveCounter.OnObjectDrop += StoveCounter_OnObjectDrop;
        //垃圾桶
        TrashCounter.OnTrashing += TrashCounter_OnTrashing;
        //容器柜子
        ContainerCounter.OnGetObject += ContainerCounter_OnGetObject;
        //玩家移动
        Player.OnMovement += Player_OnMovement;
    }

    private void CuttingCounter_OnObjectDrop(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipSo.objectDrop, cuttingCounter.transform.position, soundVolume);
    }

    private void CuttingCounter_OnObjectPickedUp(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipSo.objectPickup, cuttingCounter.transform.position, soundVolume);
    }

    private void TrashCounter_OnTrashing(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = (TrashCounter)sender;
        PlaySound(audioClipSo.trash,trashCounter.transform.position, soundVolume);
    }

    private void StoveCounter_OnObjectDrop(object sender, System.EventArgs e)
    {
        StoveCounter stoveCounter = sender as StoveCounter;
        PlaySound(audioClipSo.objectDrop, stoveCounter.transform.position, soundVolume);
    }

    private void StoveCounter_OnObjectPickedUp(object sender, System.EventArgs e)
    {
        StoveCounter stoveCounter = sender as StoveCounter;
        PlaySound(audioClipSo.objectPickup, stoveCounter.transform.position, soundVolume); 
    }

    private void CuttingCounter_OnChoping(object sender, System.EventArgs e)
    {
        CuttingCounter cuttinGCounter = (CuttingCounter)sender;
        PlaySound(audioClipSo.chop,cuttinGCounter.transform.position, soundVolume);
    }

    private void Player_OnMovement(object sender, System.EventArgs e)
    {
        Player player = (Player)sender;
        PlaySound(audioClipSo.footstep,player.transform.position,0.9f * soundVolume);
    }

    private void ContainerCounter_OnGetObject(object sender, System.EventArgs e)
    {
        ContainerCounter containerCounter = sender as ContainerCounter;
        PlaySound(audioClipSo.objectPickup, containerCounter.transform.position, soundVolume);
    }

    private void BaseCounter_OnObjectDrop(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipSo.objectDrop, baseCounter.transform.position, soundVolume);
    }

    private void BaseCounter_OnObjectPickedUp(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipSo.objectPickup,baseCounter.transform.position,soundVolume);
    }

    private void OrderManager_OnRecipeSuccessed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipSo.deliverySuccess,OrderManager.Instance.transform.position,soundVolume);
    }

    private void OrderManager_OnDeliveryFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipSo.deliveryFail,OrderManager.Instance.transform.position,soundVolume);
    }

    private void Instance_OnRecipeOverTime(object sender, System.EventArgs e)
    {
        PlaySound(audioClipSo.warning, OrderManager.Instance.transform.position, soundVolume);
    }

    private void PlaySound(AudioClip[] audioClip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClip[Random.Range(0,audioClip.Length)], position);
    }

    private void PlaySound(AudioClip[] audioClip, Vector3 position,float volume)
    {
        AudioSource.PlayClipAtPoint(audioClip[Random.Range(0, audioClip.Length)], position,volume * soundMultiple * (isSoundOn ? 1 : 0));
    }

    private void PlaySound(AudioClip audioClip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClip,position);
    }

    public void PlayWarning(Vector3 pos)
    {
        PlaySound(audioClipSo.warning, pos, soundVolume);
    }

    public void PlayFootStep(Vector3 position,float volume)
    {
        PlaySound(audioClipSo.footstep, position, volume * soundVolume);
    }

    public void PlaySound(bool isOn)
    {
        isSoundOn = isOn;   
    }

    public void SetSoundVolume(float v)
    { 
        this.soundVolume = v;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
