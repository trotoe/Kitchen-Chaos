using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryUI : MonoBehaviour
{

    [SerializeField] private GameObject successUI;
    [SerializeField] private GameObject defaultUI;

    [SerializeField] private Animator successAnimator;
    [SerializeField] private Animator deafultAnimator;

    private void Start()
    {
        OrderManager.Instance.OnRecipeSuccessed += OrderManager_OnRecipeSuccessed;
        OrderManager.Instance.OnRecipeFailed += OrderManager_OnRecipeFailed;
    }

    private void OrderManager_OnRecipeSuccessed(object sender, System.EventArgs e)
    {
        successUI.SetActive(true);
        successAnimator.SetTrigger("isDelivery");
    }

    private void OrderManager_OnRecipeFailed(object sender, EventArgs e)
    {
        defaultUI.SetActive(true);
        deafultAnimator.SetTrigger("isDelivery");
    }
}
