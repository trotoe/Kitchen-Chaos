using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : KitchenObjectHolder
{
    public static Player Instance { get; set; }

    public static event EventHandler OnMovement;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask counterLayerMask;

    private BaseCounter selectCounter;

    private Vector3 moveVector;
    private bool isWalking;
    
    public bool IsWalking { get { return isWalking; } }

    private Player() { }
    
    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
        // 锁定Rigidbody的旋转，防止物理引擎干扰
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void FixedUpdate()
    {     
        CheckBorder();
        HandleMovement();
    }

    private void Start()
    {
        GameInput.Instance.OnInteract += InstanceOnOnInteract;
        GameInput.Instance.OnOperate += InstanceOnOnOperate;
    }

    private void Update()
    {
        HandleInteraction();
    }

    //边界检测
    private void CheckBorder()
    {
        Vector3 pos = rb.position;
        pos.y = 0f;
        if (pos.x < MapBorder.Instance.GetLeftPoint().x)
        { 
            pos.x = MapBorder.Instance.GetLeftPoint().x;
        }
        else if (pos.x > MapBorder.Instance.GetRightPoint().x)
        { 
            pos.x = MapBorder.Instance.GetRightPoint().x;
        }
        if (pos.z > MapBorder.Instance.GetTopPoint().z)
        {
            pos.z = MapBorder.Instance.GetTopPoint().z;
        }
        else if (pos.z < MapBorder.Instance.GetBottomPoint().z)
        { 
            pos.z = MapBorder.Instance.GetBottomPoint().z;
        }
        rb.MovePosition(pos);
    }

    private void InstanceOnOnInteract(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        selectCounter?.Interact(this);
    }
    
    private void InstanceOnOnOperate(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        selectCounter?.Operate(this);
    }

    //处理交互逻辑
    private void HandleInteraction()
    {   
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 2f,counterLayerMask))
        {
            if (hitInfo.transform.TryGetComponent<BaseCounter>(out BaseCounter counter))
            {
                SetSelectedCounter(counter);
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter( BaseCounter counter)
    {
        if (counter != selectCounter)
        {
            selectCounter?.Deselect();
            counter?.Select();
            this.selectCounter = counter;
        }
    }
    
    //处理移动逻辑
    private void HandleMovement()
    {
        moveVector = GameInput.Instance.GetMovementDirectionNormalized();
        isWalking = moveVector != Vector3.zero;
        // this.transform.position += moveVector * (moveSpeed * Time.deltaTime);//移动
        rb.MovePosition(rb.position + moveVector * (moveSpeed * Time.fixedDeltaTime));  //移动
        if (moveVector == Vector3.zero) //如果没有移动，则停止移动，防止物理引擎干扰
        { 
            rb.velocity = Vector3.zero;
        }
        else
        {
            transform.forward = Vector3.Slerp(transform.forward, moveVector, rotateSpeed * Time.deltaTime);//使面朝向与移动方向一致
        }
    }

    private void OnDestroy()
    {
        Instance = null;
        OnMovement = null;
    }
}
