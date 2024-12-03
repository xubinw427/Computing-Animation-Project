using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Play the waving animation when the Space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play("RodWaving"); // Animation name must match the one in Animator
        }
    }
}
