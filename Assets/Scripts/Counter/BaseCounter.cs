using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : KitchenObjectHolder
{
    public static event EventHandler OnObjectPickedUp;
    public static event EventHandler OnObjectDrop;

    [SerializeField]private GameObject selected;
    public virtual void Interact(Player player)     //F键
    {
        if (player.IsHaveKitchenObject()) //玩家手里有物体
        {
            if (player.GetKitchenObject().TryGetComponent<Plate>(out Plate plate))  //玩家手里有盘子
            {
                if (this.IsHaveKitchenObject()) //柜台上有食物
                {
                    if (plate.TryAddKitchenObject(GetKitchenObjectSo()))
                    {
                        OnObjectPickedUp?.Invoke(this, EventArgs.Empty);
                        DestoryKitchenObject();
                    }
                }
                else //柜台上没有东西
                {
                    TransferKitchenObject(player, this);
                    OnObjectDrop?.Invoke(this, EventArgs.Empty);
                }
            }
            else    //玩家手里是食物
            {
                if (this.IsHaveKitchenObject()) //柜台上有盘子
                {
                    if (GetKitchenObject().TryGetComponent<Plate>(out plate))
                    {
                        if (plate.TryAddKitchenObject(player.GetKitchenObjectSo()))
                        {
                            OnObjectPickedUp?.Invoke(this,EventArgs.Empty);
                            player.DestoryKitchenObject();
                        }
                    }
                }
                else //柜台上没有食物
                {
                    TransferKitchenObject(player, this);
                    OnObjectDrop?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        else //玩家手里没有物体
        {
            if (this.IsHaveKitchenObject()) //柜台上有物体
            {
                TransferKitchenObject(this, player);
                OnObjectPickedUp?.Invoke(this, EventArgs.Empty);
            }
            else //柜台上没有食物
            {

            }
        }
    }

    public virtual void Operate(Player player)  //E键
    {
        
    }
    
    public void Select()
    {
        selected.SetActive(true);
    }
    
    public void Deselect()
    {
        selected.SetActive(false);
    }

    private void OnDestroy()
    {
        OnObjectPickedUp = null;
        OnObjectDrop = null;
    }
}
