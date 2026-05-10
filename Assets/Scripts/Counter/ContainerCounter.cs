using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//储藏柜类
public class ContainerCounter : BaseCounter
{
    public static event EventHandler OnGetObject;

    [SerializeField]private KitchenObjectSo kitchenObjectSo;
    [SerializeField]private CounterAnimator animator;
    [SerializeField]private CounterAnimator selectedAnimator;   
    
    public override void Interact(Player player)
    {
        base.Interact(player);
        Operate(player);
    }
    
    public override void Operate(Player player)
    {
        if (player.IsHaveKitchenObject() || IsHaveKitchenObject()) return;
        
        CreatKitchObject(kitchenObjectSo.prefab);
        print(this.GetKitchenObject());  
        TransferKitchenObject(this,player);
        OnGetObject?.Invoke(this,EventArgs.Empty);
        animator.PlayOpen();
        selectedAnimator.PlayOpen();
    }

    private void OnDestroy()
    {
        OnGetObject = null;
    }
}
