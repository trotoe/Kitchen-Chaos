using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingAnimator : MonoBehaviour
{
    [SerializeField]private GameObject sizzling;
    [SerializeField]private GameObject stove;

    public void ShowEffect()
    {
        stove.SetActive(true);
        sizzling.SetActive(true);
    }

    public void HideEffect()
    {
        stove.SetActive(false);
        sizzling.SetActive(false);
    }
    
    
}
