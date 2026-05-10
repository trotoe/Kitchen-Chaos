using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeTimeUI : MonoBehaviour
{
    int id = 0;
    private Image imgTime;
    private float time;
    private float warnningTime;
    private float maxTime;

    public void Init(int id,float time)
    {
        this.id = id;
        imgTime = GetComponent<Image>();
        maxTime = OrderManager.Instance.RecipeMaxTime;
        warnningTime = maxTime * (1f / 3);
        this.time = time;
    }

    private void Update()
    {
        time = OrderManager.Instance.GetRecipeTimeList()[id];
        float progress = imgTime.fillAmount = time / maxTime;
        if (time < warnningTime)
        {
            imgTime.color = Color.red;
        }
    }
}

