using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoveSound : MonoBehaviour
{
    [SerializeField]private AudioSource audioSource;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        StoveCounter.OnStoveSizzle += StoveCounter_OnStoveSizzle;
    }

    private void StoveCounter_OnStoveSizzle(object sender, OnStoveSizzleAegs e)
    {
        bool playeSound = (e.isCooking && (stoveCounter.IsHaveKitchenObject()));
        if (playeSound)
        {
            audioSource.Play();
        }
        else
        { 
            audioSource.Pause();
        }
    }

    private void OnDestroy()
    {
        StoveCounter.OnStoveSizzle -= StoveCounter_OnStoveSizzle;
    }
}
