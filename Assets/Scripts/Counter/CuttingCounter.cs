using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public static event EventHandler OnChoping;
    public static new event EventHandler OnObjectPickedUp;
    public static new event EventHandler OnObjectDrop;

    [SerializeField]private CuttingRecipeListSo cuttingRecipesListSo;
    [SerializeField]private CuttingAnimator cuttingAnimator;
    
    public override void Interact(Player player)
    {
        if (player.IsHaveKitchenObject()) //玩家手里有物体
        {
            if (player.GetKitchenObject().TryGetComponent<Plate>(out Plate plate))  //玩家手里有盘子
            {
                if (this.IsHaveKitchenObject()) //柜台上有食物
                {
                    if (plate.TryAddKitchenObject(GetKitchenObjectSo()))
                    {
                        DestoryKitchenObject();
                    }
                }
            }
            else    //玩家手里是食物
            {
                TransferKitchenObject(player,this);
            }
        }
        else //玩家手里没有物体
        {
            if (this.IsHaveKitchenObject()) //柜台上有物体
            {
                TransferKitchenObject(this,player);
            }
            else //柜台上没有食物
            {
                
            }
        }
    }

    public override void Operate(Player player)
    {
        if (IsHaveKitchenObject())
        { 
            KitchenObjectSo output = cuttingRecipesListSo.GetOutput(GetKitchenObject().GetKitchenObjectSo());
            if (cuttingRecipesListSo.TryGetCuttingRecipe(GetKitchenObject().GetKitchenObjectSo(), out CuttingRecipe recipe))
            {
                GetKitchenObject()?.GetProgressBarUI().Show();
                cuttingAnimator.PlayCutting();
                OnChoping?.Invoke(this,EventArgs.Empty);
                GetKitchenObject().cuttingCount++;
                GetKitchenObject()?.GetProgressBarUI().UpdateProgress((float)GetKitchenObject().cuttingCount / recipe.cuttingCountMax);
                if (GetKitchenObject().cuttingCount == recipe.cuttingCountMax)
                {
                    GetKitchenObject()?.GetProgressBarUI().Hide();
                    DestoryKitchenObject();
                    CreatKitchObject(output.prefab);
                }
            }
        }
    }

    private void OnDestroy()
    {
        OnChoping = null;
        OnObjectPickedUp = null;
        OnObjectDrop = null;
    }
}
