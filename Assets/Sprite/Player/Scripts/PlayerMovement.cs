using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float runSpeed = 40f;
    private Animator animator;

    float horizontalMove = 0f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal")*runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    void FixedUpdate()
    {
        //move our character
        controller.Move(horizontalMove*Time.fixedDeltaTime, false, false);
    }
}
