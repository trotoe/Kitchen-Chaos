using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField]private KitchenObjectSo plateSo;
    [SerializeField]private int maxPlateCount = 7;
    [SerializeField]private float deltaTime = 3f;
    
    private float time = 0f;
    private List<KitchenObject> platesList = new List<KitchenObject>();
    
    public override void Interact(Player player)
    {
        if (!player.IsHaveKitchenObject())       //玩家手里没有物品
        {
            if (platesList.Count <= 0) return;
            player.AddKitchenObject(platesList[platesList.Count - 1]);
            platesList.RemoveAt(platesList.Count - 1);
        }
        else //玩家手里有物体
        {
            if(platesList.Count <= 0)   return;
            Plate plate = platesList[platesList.Count - 1] as Plate;
            if (platesList.Count > 0 && plate.TryAddKitchenObject(player.GetKitchenObjectSo()))   //玩家手里有可以放在盘子上的物体
            {
                player.DestoryKitchenObject();
                player.AddKitchenObject(platesList[platesList.Count - 1]);
                platesList.RemoveAt(platesList.Count - 1);
            }
            else    //玩家手里没有可以放在盘子上的物体
            {
                return;
            }
        }
    }
    
    private void Update()
    {
        if (platesList.Count >= maxPlateCount) return;
        time += Time.deltaTime;
        if (time >= deltaTime)
        {
            time = 0f;
            SpwanPlate();
        }
    }

    private void SpwanPlate()
    {
        KitchenObject kitchenObject = Instantiate(plateSo.prefab, GetHoldPoint()).GetComponent<KitchenObject>();
        kitchenObject.transform.localPosition = Vector3.up * (0.1f * platesList.Count);
        platesList.Add(kitchenObject);
    }
}
