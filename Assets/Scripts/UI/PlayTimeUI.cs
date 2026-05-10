using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayTimeUI : MonoBehaviour
{
    [SerializeField] private Image playTimeUI;
    [SerializeField] private TextMeshProUGUI txtTimeUI;
    [SerializeField] private Image BGImg;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        float progress = playTimeUI.fillAmount = GameManager.Instance.ShowPlayTimeImg();
        txtTimeUI.text = Mathf.CeilToInt(GameManager.Instance.ShowPlayTimeTxt()).ToString();
        if (1 - progress < (1f / 5))
        { 
            BGImg.color = Color.red;
        }
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
