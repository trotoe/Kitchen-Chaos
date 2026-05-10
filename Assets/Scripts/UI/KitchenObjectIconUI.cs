using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenObjectIconUI : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    /*public void Hide()
    {
        
    }*/
}
