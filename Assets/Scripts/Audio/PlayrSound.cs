using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//将此脚本挂在空物体身上，放到相机附近，使玩家的脚步声更均匀
public class PlayrSound : MonoBehaviour
{
    private float footstepTimer = 0f;
    private float footstepTimerMax = 0.1f;

    [SerializeField] private float offsetPos = 3f;

    private void Update()
    {
        this.transform.position = Camera.main.transform.position + (Vector3.up * offsetPos);
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0)
        {
            footstepTimer = footstepTimerMax;
            if (Player.Instance != null && Player.Instance.IsWalking)
            {
                SoundManager.Instance.PlayFootStep(this.transform.position, 1f);
            }
        }
    }
}
