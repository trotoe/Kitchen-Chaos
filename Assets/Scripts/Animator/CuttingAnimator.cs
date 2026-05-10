using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingAnimator : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayCutting()
    {
        animator.SetTrigger("Cut");
    }
}
