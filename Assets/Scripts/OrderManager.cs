using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFailed;
    public event EventHandler OnRecipeSuccessed;
    public event EventHandler OnRecipeOverTime;

    [SerializeField]private RecipeListSo menu;
    [SerializeField]private float orderRate = 5f;
    [SerializeField]private int orderCountMax = 5;

    private List<RecipeSo> orderMenu = new List<RecipeSo>();
    private int orderCount = 0;
    private float orderTime = 0f;
    private bool isStarting = true;
    private int recipeSuccessfulCount = 0;

    private List<float> recipeTimeList = new List<float>(); 
    private float recipeMaxTime = 45f;

    public float RecipeMaxTime => recipeMaxTime;

    private OrderManager()
    {
        
    }
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        isStarting = orderCount >= orderCountMax ? false : true;
        if (isStarting)
        {
            orderTime += Time.deltaTime;
            if (orderTime >= orderRate)
            {
                orderTime = 0f;
                orderRate = UnityEngine.Random.Range(8f, 10f);
                int index = UnityEngine.Random.Range(0, menu.recipeListSo.Count);
                SpawnOrder(index);
            }
        }
        if (orderMenu.Count > 0)
        {
            for (int i = 0; i < recipeTimeList.Count; i++)
            {
                SpawnRecipe(i);
            }
        }
    }

    private void SpawnOrder(int index)
    {
        orderMenu.Add(menu.recipeListSo[index]);
        recipeTimeList.Add(recipeMaxTime);
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        orderCount++;
    }

    private void SpawnRecipe(int index)
    {
        recipeTimeList[index] -= Time.deltaTime;

        if (recipeTimeList[index] <= 0f)
        {
            orderMenu.RemoveAt(index);
            recipeTimeList.RemoveAt(index);
            orderCount--;
            OnRecipeOverTime?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DeliveryRecipe(Plate plate)
    { 
        for(int i = 0;i < orderMenu.Count;i++)
        {
            if (IsCorrect(plate, orderMenu[i]))
            {
                DeliveryCorrectRecipe(i);
                return;
            }
        }
        DeliveryDefaultRecipe();
    }

    private void DeliveryCorrectRecipe(int index)
    {
        orderMenu.Remove(orderMenu[index]);
        recipeTimeList.RemoveAt(index);
        orderCount--;
        recipeSuccessfulCount++;
        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccessed?.Invoke(this, EventArgs.Empty);
        print("上菜成功");
    }

    private void DeliveryDefaultRecipe()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        print("上菜失败");
    }

    private bool IsCorrect(Plate plate,RecipeSo orderRecipe)
    {
        List<KitchenObjectSo> recipe = plate.GetKitchenObjectSoList();
        if(recipe.Count != orderRecipe.kitchenObjectSoList.Count) return false;
        foreach (KitchenObjectSo foodSo in recipe)
        {
            if (!orderRecipe.kitchenObjectSoList.Contains(foodSo))
            { 
                return false;
            }
        }
        return true;
    }

    public List<RecipeSo> GetRecipeList()
    { 
        return orderMenu;
    }

    public int GetRecipeSuccessFulCount()
    {
        return recipeSuccessfulCount;
    }

    public List<float> GetRecipeTimeList()
    { 
        return recipeTimeList;
    }

    public void OnDestroy()
    {
        Instance = null;
        OnRecipeSpawned = null;
        OnRecipeCompleted = null;
        OnRecipeFailed = null;
        OnRecipeSuccessed = null;
        OnRecipeOverTime = null;
    }
}
