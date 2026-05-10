using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField]private Image image;
    [SerializeField]private KitchenObject kitchenObject;
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (kitchenObject.isFinishing)
        {
            gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void UpdateProgress(float progress)
    {
        image.fillAmount = progress;
    }
}
