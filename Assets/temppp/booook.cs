using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class booook : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.SetTrigger("do");
        }
    }
}
