using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectGridUI : MonoBehaviour
{
    [SerializeField]private KitchenObjectIconUI templateIconUI;
    [SerializeField]private RectTransform rectTransform;
    private byte kitchenObjectCount;

    void Start()
    {
        kitchenObjectCount = 0;
    }

    public void ShowIcon(KitchenObjectSo kitchenObjectSo)
    {
        KitchenObjectIconUI kitchenObjectIconUI = Instantiate(templateIconUI, transform);
        kitchenObjectIconUI.SetIcon(kitchenObjectSo.sprite);
        kitchenObjectCount++;
        if (kitchenObjectCount > 3)
        {
            rectTransform.localPosition = new Vector3(0, 1.5f, 0);
        }
    }
}
