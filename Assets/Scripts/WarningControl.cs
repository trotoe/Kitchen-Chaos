using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningControl : MonoBehaviour
{
    [SerializeField] private GameObject warningBar;
    [SerializeField] private Animator progressBarAnimator;

    private bool isStart = false;

    [SerializeField] private float soundRate = 0.2f;
    private float time = 0f;

    private void Update()
    {
        if (!isStart) return;
        time += Time.deltaTime;
        if (time >= soundRate)
        {
            SoundManager.Instance.PlayWarning(this.transform.position);
            time = 0f;
        }
    }

    public void StartWarning()
    {
        if (isStart) return;
        isStart = true;
        warningBar.SetActive(true);
        progressBarAnimator.SetBool("isWarning", true);
    }

    public void StopWarning()
    {
        isStart = false;
        warningBar.SetActive(false);
        progressBarAnimator.SetBool("isWarning", false);
    }
}

