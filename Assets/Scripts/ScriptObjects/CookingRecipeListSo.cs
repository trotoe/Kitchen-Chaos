using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CookingRecipe
{
    public KitchenObjectSo input;
    public KitchenObjectSo output;
    public float cookingTime;
}

[CreateAssetMenu()]
public class CookingRecipeListSo : ScriptableObject
{
    public List<CookingRecipe> cookingRecipeList;

    public KitchenObjectSo GetOutput(KitchenObjectSo input)
    {
        foreach (CookingRecipe recipe in cookingRecipeList)
        {
            if (recipe.input == input)
            {
                return recipe.output;
            }
        }
        return null;
    }

    public bool TryGetCookingRecipe(KitchenObjectSo input, out CookingRecipe cookingRecipe)
    {
        foreach (CookingRecipe recipe in cookingRecipeList)
        {
            if (recipe.input == input)
            {
                cookingRecipe = recipe;
                return true;
            }
        }
        cookingRecipe = null;
        return false;
    }
}
