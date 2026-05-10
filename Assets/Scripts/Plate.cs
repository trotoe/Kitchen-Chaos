using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : KitchenObject
{
    [SerializeField]private List<KitchenObjectSo> validKitchenObject = new List<KitchenObjectSo>();
    [SerializeField]private PlateCompleteVisual visual;
    [SerializeField]private GameObject plateComplete;
    [SerializeField]private KitchenObjectGridUI kitchenObjectGridUI;
    
    private SortedList<int,KitchenObjectSo> kitchenObjectList = new SortedList<int, KitchenObjectSo>();

    public bool TryAddKitchenObject(KitchenObjectSo kitchenObjectSo)
    {
        if (validKitchenObject.Contains(kitchenObjectSo) && !kitchenObjectList.ContainsKey(kitchenObjectSo.priority))
        {
            if (kitchenObjectSo.type == E_Type.meat)
            {
                foreach (var temp in kitchenObjectList)
                {
                    if (temp.Value.type == E_Type.meat)
                    {
                        return false;
                    }
                }   
            }
            kitchenObjectList.Add(kitchenObjectSo.priority, kitchenObjectSo);
            visual.ShowKitchenObjectSo(kitchenObjectList);
            kitchenObjectGridUI.ShowIcon(kitchenObjectSo);
            return true;
        }
        return false;
    }

    public List<KitchenObjectSo> GetKitchenObjectSoList()
    { 
        List<KitchenObjectSo> list = new List<KitchenObjectSo>();
        foreach (var temp in this.kitchenObjectList)
        {
            list.Add(temp.Value);
        }
        return list;
    }
}
