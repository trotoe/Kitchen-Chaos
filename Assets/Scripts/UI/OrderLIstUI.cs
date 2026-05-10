using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderLIstUI : MonoBehaviour
{
    [SerializeField] private Transform recipeParent;
    [SerializeField] private RecipeUI recipeUITemplate;

    private void Awake()
    {
        recipeUITemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        OrderManager.Instance.OnRecipeSpawned += OrderManger_OnOnRecipeSpawned;
        OrderManager.Instance.OnRecipeCompleted += OrderManger_OnRecipeCompleted;
        OrderManager.Instance.OnRecipeOverTime += OrderManger_OnRecipeOverTime;
        UpdateUI();
    }

    private void OrderManger_OnRecipeOverTime(object sender, EventArgs e)
    {
        UpdateUI();
    }

    private void OrderManger_OnOnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateUI();
    }

    private void OrderManger_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Transform child in recipeParent)       //清空，防止生成重复菜单
        {
            if (child != recipeUITemplate.transform)    
            {
                Destroy(child.gameObject); 
            }
        }
        List<RecipeSo> list = OrderManager.Instance.GetRecipeList();
        List<float> timeList = OrderManager.Instance.GetRecipeTimeList();
        for(int i = 0;i < list.Count;i++)
        { 
            RecipeSo child = list[i];
            RecipeUI recipeUI = Instantiate(recipeUITemplate,recipeParent);
            recipeUI.UpdateUI(child, i, timeList[i]);
            recipeUI.gameObject.SetActive(true);
        }
    }
}
