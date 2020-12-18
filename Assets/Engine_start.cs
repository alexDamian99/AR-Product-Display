using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine_start : MonoBehaviour
{
    private Animator animator;
    private bool start = false;
    
    void Start()
    {
        animator = GetComponent < Animator>();
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !start)
        {
            animator.SetBool("GasOn", true);
            start = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && start)
        {
            animator.SetBool("GasOn", false);
            start = false;
        }
    }
}
