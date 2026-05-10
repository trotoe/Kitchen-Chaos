using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StoveCounter;

public class OnStoveSizzleAegs : EventArgs
{
    public bool isCooking;
}

public class StoveCounter : BaseCounter
{
    public enum E_CookingState
    {
        idle,
        frying,
        burning,
    }

    [SerializeField] private WarningControl warningControl;

    public static new event EventHandler OnObjectPickedUp;
    public static new event EventHandler OnObjectDrop;
    public static event EventHandler OnWarning;
    public static event EventHandler<OnStoveSizzleAegs> OnStoveSizzle;

    [SerializeField]private CookingRecipeListSo fryingRecipes;
    [SerializeField]private CookingRecipeListSo burningRecipes;
    [SerializeField]private CookingAnimator visual;
    
    private bool isCooking = false;
    private CookingRecipe fRecipe;
    private CookingRecipe bRecipe;
    private E_CookingState cookingState;

    private void Update()
    {
        if (IsHaveKitchenObject() && isCooking)
        {
            if (cookingState == E_CookingState.frying)
            {
                GetKitchenObject().GetProgressBarUI().Show();
                GetKitchenObject().cookingTime += Time.deltaTime;
                GetKitchenObject().GetProgressBarUI().UpdateProgress(GetKitchenObject().cookingTime / fRecipe.cookingTime);
                if (GetKitchenObject().cookingTime >= fRecipe.cookingTime)
                {
                    DestoryKitchenObject();
                    CreatKitchObject(fRecipe.output.prefab);
                    fRecipe = null;
                    if ( burningRecipes.TryGetCookingRecipe(GetKitchenObject().GetKitchenObjectSo(),
                            out bRecipe))
                    {
                        cookingState = E_CookingState.burning;
                    }
                }
            }
            else if (cookingState == E_CookingState.burning)
            {
                GetKitchenObject()?.GetProgressBarUI().Show();
                GetKitchenObject().cookingTime += Time.deltaTime;
                GetKitchenObject()?.GetProgressBarUI().UpdateProgress(GetKitchenObject().cookingTime / bRecipe.cookingTime);
                if ((GetKitchenObject().cookingTime / bRecipe.cookingTime) > 0.5f)
                {
                    this.GetKitchenObject()?.GetWarningControl().StartWarning();
                }
                if (GetKitchenObject().cookingTime >= bRecipe.cookingTime)
                {
                    DestoryKitchenObject();
                    CreatKitchObject(bRecipe.output.prefab);
                    cookingState = E_CookingState.idle;
                }
            }
        }
    }

    public override void Interact(Player player)
    {
        if (player.IsHaveKitchenObject()) //玩家手里有物体
        {
            if (player.GetKitchenObject().TryGetComponent<Plate>(out Plate plate))  //如果是盘子
            {
                if (plate.TryAddKitchenObject(GetKitchenObjectSo()))
                {
                    OnObjectPickedUp?.Invoke(this,EventArgs.Empty);
                    DestoryKitchenObject();
                }
            }
            else if (!this.IsHaveKitchenObject() 
                && (fryingRecipes.TryGetCookingRecipe(player.GetKitchenObject().GetKitchenObjectSo()
                        ,out  fRecipe) 
                    || burningRecipes.TryGetCookingRecipe(player.GetKitchenObject().GetKitchenObjectSo()
                        ,out  bRecipe)))  //如果是食物 && 柜台上没有食物
            {
                TransferKitchenObject(player,this);
                this.GetKitchenObject().GetProgressBarUI().Show();
                OnObjectDrop?.Invoke(this,EventArgs.Empty);
                cookingState = fRecipe == null ? (bRecipe == null ? E_CookingState.idle : E_CookingState.burning) : E_CookingState.frying;
            }
            OnStoveSizzle?.Invoke(this, new OnStoveSizzleAegs { isCooking = this.isCooking });
        }
        else //玩家手里没有食材
        {
            if (this.IsHaveKitchenObject()) //柜台上有食物
            {
                TransferKitchenObject(this,player);
                OnObjectPickedUp?.Invoke(this,EventArgs.Empty);
                fRecipe = null;
                bRecipe = null;
                cookingState = E_CookingState.idle;
                OnStoveSizzle?.Invoke(this, new OnStoveSizzleAegs { isCooking = this.isCooking });
                player.GetKitchenObject()?.GetProgressBarUI().Hide();
                player.GetKitchenObject()?.GetWarningControl()?.StopWarning();
            }
            else //柜台上没有食物
            {
                
            }
        }
    }

    public override void Operate(Player player)
    {
        isCooking = !isCooking;
        Cooking(isCooking);
    }

    private void Cooking(bool cookingState)
    {
        if (cookingState)
        {
            visual.ShowEffect();
        }
        else
        {
            visual.HideEffect();
        }
        OnStoveSizzle?.Invoke(this, new OnStoveSizzleAegs { isCooking = this.isCooking });
    }

    private void OnDestroy()
    {
        OnObjectPickedUp = null;
        OnObjectDrop = null;
        OnStoveSizzle = null;
    }
}
