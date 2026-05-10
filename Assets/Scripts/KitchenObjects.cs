using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] protected KitchenObjectSo kitchenObjectSo;
    [SerializeField] protected ProgressBarUI progressBarUI;
    [SerializeField] protected WarningControl warningControl;
    public int cuttingCount = 0;
    public float cookingTime = 0f;
    //public bool isHandling = false;
    public bool isFinishing = false;
    
    public KitchenObjectSo GetKitchenObjectSo()
    {
        return kitchenObjectSo;
    }

    public ProgressBarUI GetProgressBarUI()
    {
        return progressBarUI;
    }

    public WarningControl GetWarningControl()
    {
        return warningControl;
    }
}
