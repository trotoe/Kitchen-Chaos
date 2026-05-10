using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string Is_Walking = "isWalking";
    
    [SerializeField] private Player player;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(Is_Walking,player.IsWalking);
    }
}
