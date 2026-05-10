using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtUp;
    [SerializeField] TextMeshProUGUI txtDown;
    [SerializeField] TextMeshProUGUI txtLeft;
    [SerializeField] TextMeshProUGUI txtRight;
    [SerializeField] TextMeshProUGUI txtInteract;
    [SerializeField] TextMeshProUGUI txtOperate;
    [SerializeField] TextMeshProUGUI txtPause;
    [SerializeField] TextMeshProUGUI txtContinue;

    private void Awake()
    {
        GameInput.Instance.OnResume += Resume;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.GetGameState() == GameManager.E_GameState.TutorialToWaits) return;
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        { 
            this.gameObject.SetActive(false);
        }
    }

    public void Show()
    { 
        UpdateTutorialKey();
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void Resume(object sender, EventArgs e)
    {
        if (GameManager.Instance != null)
        { 
            GameManager.Instance.SetGamePause(false);
        }
        Hide();    
    }

    private void UpdateTutorialKey()
    {
        txtUp.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Up);
        txtLeft.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Left);
        txtDown.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Down);
        txtRight.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Right);
        txtInteract.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Interact);
        txtOperate.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Operate);
        txtPause.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Pause);
        txtContinue.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Interact);
    }
}
