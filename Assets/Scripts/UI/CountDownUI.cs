using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;

    private Animator animator;

    private int perNum = -1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        GameManager.Instance.OnStateChanged += GmaeManager_OnStateChanged;
        Hide();
    }

    private void GmaeManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountDownToStart())
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
        countDownTime();
    }

    private void Show()
    { 
        this.gameObject.SetActive(true);
    }

    private void Hide()
    { 
        this.gameObject.SetActive(false);
    }

    private void countDownTime()
    {
        int num = Mathf.CeilToInt(GameManager.Instance.GetCountDownTime());
        if (num != perNum)
        {
            animator.SetTrigger("Shake");
        }
        perNum = num;
        textUI.text = perNum.ToString();
    }
}
