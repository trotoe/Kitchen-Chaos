using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Transform iconParent;
    [SerializeField] private Image iconTemplate;
    [SerializeField] private RecipeTimeUI recipeTimeUI;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void UpdateUI(RecipeSo recipe,int id,float time)
    { 
        recipeName.text = recipe.recipeName;
        foreach (KitchenObjectSo kitchenObject in recipe.kitchenObjectSoList)
        {
            Image icon = Instantiate(iconTemplate,iconParent);
            icon.sprite = kitchenObject.sprite;
            icon.gameObject.SetActive (true);
        }
        recipeTimeUI.Init(id, time);
    }
}
