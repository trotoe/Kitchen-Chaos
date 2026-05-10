using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameInput;

public class CustomKeyUI : MonoBehaviour
{
    //重绑定按键关联UI
    //上
    [SerializeField] Button btnUp;
    [SerializeField] TextMeshProUGUI txtUp;
    //下
    [SerializeField] Button btnDown;
    [SerializeField] TextMeshProUGUI txtDown;
    //左
    [SerializeField] Button btnLeft;
    [SerializeField] TextMeshProUGUI txtLeft;
    //右
    [SerializeField] Button btnRight;
    [SerializeField] TextMeshProUGUI txtRight;
    //柜台交互
    [SerializeField] Button btnOperate;
    [SerializeField] TextMeshProUGUI txtOperate;
    //拿放物体
    [SerializeField] Button btnInteract;
    [SerializeField] TextMeshProUGUI txtInteract;
    //暂停
    [SerializeField] Button btnPause;
    [SerializeField] TextMeshProUGUI txtPause;
    
    //确认按键
    [SerializeField] Button btnConfirm;

    [SerializeField] GameObject tipsPanel;

    public void Awake()
    {
        btnConfirm.onClick.AddListener(() =>
        {
            Hide();
        });

        btnUp.onClick.AddListener(() => { ReBinding(GameInput.E_BindingType.Up); } );
        btnDown.onClick.AddListener(() => { ReBinding(GameInput.E_BindingType.Down); });
        btnLeft.onClick.AddListener(() => { ReBinding(GameInput.E_BindingType.Left); });
        btnRight.onClick.AddListener(() => { ReBinding(GameInput.E_BindingType.Right); });
        btnOperate.onClick.AddListener(() => { ReBinding(GameInput.E_BindingType.Operate); });
        btnInteract.onClick.AddListener(() => { ReBinding(GameInput.E_BindingType.Interact); });
        btnPause.onClick.AddListener(() => { ReBinding(GameInput.E_BindingType.Pause); });
    }

    public void Show()
    {
        UpdateBindingText();
        gameObject.SetActive(true);
    }

    private void UpdateBindingText()
    {
        txtUp.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Up);
        txtDown.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Down);
        txtLeft.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Left);
        txtRight.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Right);
        txtOperate.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Operate);
        txtInteract.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Interact);
        txtPause.text = GameInput.Instance.GetBindingString(GameInput.E_BindingType.Pause);
    }

    private void ReBinding(E_BindingType bindingType)
    {
        tipsPanel.SetActive(true);
        GameInput.Instance.ReBinding(bindingType, () =>
        {
            UpdateBindingText();
            tipsPanel.SetActive(false);
        });
}

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
