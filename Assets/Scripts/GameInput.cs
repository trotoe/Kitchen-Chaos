using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;



public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteract;
    public event EventHandler OnOperate;
    public event EventHandler OnPause;
    public event EventHandler OnResume;

    public enum E_BindingType
    { 
        Up, Down, Left, Right,
        Operate, Interact,
        Pause,
    }

    private const string GAMEINPUT_BINDINGS = "GameInputBindings";

    private Vector2 inputVector;
    private Vector3 direction;
    // private float horizontal;
    // private float vertical;

    public static GameInput Instance { get;private set; }
    private PlayerInputAction inputAction;
    
    //防止外部实例化
    private GameInput() { }

    void Awake()
    {
        if (Instance == null)
        {
            GameInput.Instance = this;
        }
        inputAction = new PlayerInputAction();
        if (PlayerPrefs.HasKey(GAMEINPUT_BINDINGS))
        { 
            inputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(GAMEINPUT_BINDINGS));
        }
        inputAction.Player.Enable();
        inputAction.Player.Interact.performed += InteractOnperformed;
        inputAction.Player.Interact.performed += ResumeOnperformed;
        inputAction.Player.Operate.performed += OperateOnperformed;
        inputAction.Player.Pause.performed += PauseOnperformed;
    }

    public void ReBinding(E_BindingType binding,Action onFinish)
    {
        inputAction.Player.Disable();
        InputAction iAction = new InputAction();
        int id = -1;
        switch (binding)
        {
            case E_BindingType.Up:
                iAction = inputAction.Player.Move;
                id = 1;
                break;
            case E_BindingType.Down:
                iAction = inputAction.Player.Move;
                id = 2;
                break;
            case E_BindingType.Left:
                iAction = inputAction.Player.Move;
                id = 3;
                break;
            case E_BindingType.Right:
                iAction = inputAction.Player.Move;
                id = 4;
                break;
            case E_BindingType.Operate:
                iAction = inputAction.Player.Operate;
                id = 0;
                break;
            case E_BindingType.Interact:
                iAction = inputAction.Player.Interact;
                id = 0;
                break;
            case E_BindingType.Pause:
                iAction = inputAction.Player.Pause;
                id = 0;
                break;
            default:
                break;
        }

        iAction.PerformInteractiveRebinding(id).OnComplete((callback)=>
        { 
            callback.Dispose();
            inputAction.Player.Enable();
            PlayerPrefs.SetString(GAMEINPUT_BINDINGS,inputAction.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            onFinish?.Invoke();
        }).Start();
    }

    public string GetBindingString(E_BindingType bindingType)
    {
        switch (bindingType)
        {
            case E_BindingType.Up:
                return inputAction.Player.Move.bindings[1].ToDisplayString();
            case E_BindingType.Down:
                return inputAction.Player.Move.bindings[2].ToDisplayString();
            case E_BindingType.Left:
                return inputAction.Player.Move.bindings[3].ToDisplayString();
            case E_BindingType.Right:
                return inputAction.Player.Move.bindings[4].ToDisplayString();
            case E_BindingType.Operate:
                return inputAction.Player.Operate.bindings[0].ToDisplayString();
            case E_BindingType.Interact:
                return inputAction.Player.Interact.bindings[0].ToDisplayString();
            case E_BindingType.Pause:
                return inputAction.Player.Pause.bindings[0].ToDisplayString();
            default:
                return null;
        }
    }

    
    public void DisableInput()
    {
        inputAction.Player.Move.Disable();
        inputAction.Player.Interact.Disable();
        inputAction.Player.Operate.Disable();
    }

    public void EnableInput()
    {
        inputAction.Player.Move.Enable();
        inputAction.Player.Interact.Enable();
        inputAction.Player.Operate.Enable();
    }

    private void ResumeOnperformed(InputAction.CallbackContext context)
    {
        OnResume?.Invoke(this, EventArgs.Empty);
    }

    private void PauseOnperformed(InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void OperateOnperformed(InputAction.CallbackContext obj)
    {
        OnOperate?.Invoke(this, EventArgs.Empty);
    }

    private void InteractOnperformed(InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetMovementDirectionNormalized()
    {
        // horizontal = Input.GetAxisRaw("Horizontal");
        // vertical = Input.GetAxisRaw("Vertical");
        // inputVector = new Vector3(horizontal,0,vertical);//读取输入方向
        // inputVector = inputVector.normalized;//单位化
        
        inputVector = inputAction.Player.Move.ReadValue<Vector2>();
        direction = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        
        return direction;
    }

    private void OnDestroy()
    {
        Instance = null;
        inputAction.Player.Interact.performed -= InteractOnperformed;
        inputAction.Player.Operate.performed -= OperateOnperformed;
        inputAction.Player.Pause.performed -= PauseOnperformed;
        inputAction.Player.Interact.performed -= ResumeOnperformed;
        inputAction.Player.Disable();
        inputAction.Disable();
        inputAction.Dispose();
        inputAction = null;
    }
}
