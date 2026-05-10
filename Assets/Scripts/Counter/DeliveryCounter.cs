using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance;

    private DeliveryCounter()
    {
        
    }

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
    }

    //public event EventHandler OnDeliverryCounter;

    public override void Interact(Player player)
    {
        if(player.IsHaveKitchenObject() && player.GetKitchenObject().TryGetComponent<Plate>(out Plate plate))
        { 
            player.DestoryKitchenObject();
            OrderManager.Instance.DeliveryRecipe(plate);
            //OnDeliverryCounter?.Invoke(this,EventArgs.Empty);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
