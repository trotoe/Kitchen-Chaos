using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    private class KitchenObjectSo_Model
    {
        public KitchenObjectSo kitchenObjectSo;
        public GameObject model;
    }
    
    [SerializeField]private List<KitchenObjectSo_Model> modelMap = new List<KitchenObjectSo_Model>();

    public void ShowKitchenObjectSo(SortedList<int,KitchenObjectSo> list)
    {
        float height = 0f;
        foreach (var food in list)
        {
            height += food.Value.height;
            modelMap[food.Key].model.SetActive(true);
            modelMap[food.Key].model.transform.localPosition = new Vector3(0f, height, 0f);
        }
    }
}
