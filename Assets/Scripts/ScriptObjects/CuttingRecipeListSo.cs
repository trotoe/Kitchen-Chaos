using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CuttingRecipe
{
    public KitchenObjectSo input;
    public KitchenObjectSo output;
    public int cuttingCountMax;
}

[CreateAssetMenu()]
public class CuttingRecipeListSo : ScriptableObject
{
    public List<CuttingRecipe> cuttingRecipeList;

    public KitchenObjectSo GetOutput(KitchenObjectSo input)
    {
        foreach (CuttingRecipe recipe in cuttingRecipeList)
        {
            if (recipe.input == input)
            {
                return recipe.output;
            }
        }
        return null;
    }

    public bool TryGetCuttingRecipe(KitchenObjectSo input, out CuttingRecipe cuttingRecipe)
    {
        foreach (CuttingRecipe recipe in cuttingRecipeList)
        {
            if (recipe.input == input)
            {
                cuttingRecipe = recipe;
                return true;
            }
        }
        cuttingRecipe = null;
        return false;
    }
}