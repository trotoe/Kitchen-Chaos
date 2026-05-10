using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectHolder : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;

    private KitchenObject kitchenObject;
    
    public KitchenObject GetKitchenObject() { return kitchenObject; }

    public KitchenObjectSo GetKitchenObjectSo() { return kitchenObject.GetKitchenObjectSo();  }

    public bool IsHaveKitchenObject() { return kitchenObject != null; }
    
    public Transform GetHoldPoint() { return holdPoint; }
    
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        kitchenObject.transform.localPosition = Vector3.zero;
    }
    
    public void TransferKitchenObject(KitchenObjectHolder sourceHolder, KitchenObjectHolder targetHolder)
    {
        if (sourceHolder.GetKitchenObject() == null)
        {
            Debug.LogWarning("原持有者上没有食材，转移食材");
            return;
        }
        if (targetHolder.GetKitchenObject() != null)
        {
            Debug.LogWarning("目标持有者上已有食材，转移食材");
            return;
        }
        targetHolder.AddKitchenObject(sourceHolder.kitchenObject);
        sourceHolder.ClearKitchenObject();
    }
    
    public void AddKitchenObject(KitchenObject kitchenObject)
    {
        kitchenObject.transform.SetParent(this.GetHoldPoint());
        kitchenObject.transform.localPosition = Vector3.zero;
        this.kitchenObject = kitchenObject;
    }

    public void CreatKitchObject(GameObject kitchenObjectPrefab)
    {
        KitchenObject kitchenObject = Instantiate(kitchenObjectPrefab, GetHoldPoint()).GetComponent<KitchenObject>();
        SetKitchenObject(kitchenObject);
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public void DestoryKitchenObject()
    {
        Destroy(GetKitchenObject().gameObject);
        ClearKitchenObject();
    }
}
