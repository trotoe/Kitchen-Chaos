using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnTrashing;

    public override void Interact(Player player)
    {
        if (player.IsHaveKitchenObject())
        {
            player.DestoryKitchenObject();
            OnTrashing?.Invoke(this,EventArgs.Empty);
        }
    }

    private void OnDestroy()
    {
        OnTrashing = null;
    }
}
