using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goback : MonoBehaviour
{
    Animator animator;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }
    private void OnDisable()
    {
        animator.SetTrigger("Back");
    }
}
