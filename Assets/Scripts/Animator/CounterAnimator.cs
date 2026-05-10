using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAnimator : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayOpen()
    {
        animator.SetTrigger("OpenClose");
    }
}
