using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeSo : ScriptableObject
{
    public string recipeName;

    public List<KitchenObjectSo> kitchenObjectSoList;
}
