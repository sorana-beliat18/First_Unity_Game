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
    bool jump = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        controller.OnLandEvent.AddListener(OnLanding);
    }

    public void OnLanding()
    {
        // Resetează imediat trigger-ul "Jump" cand personajul atinge solul.
        animator.ResetTrigger("Jump");
        // Opțional: Aici poți seta un Bool "IsJumping" pe False, dacă folosești un Bool.
    }
    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal")*runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if(Input.GetButtonDown("Jump") )
        {
            jump = true;
            animator.SetTrigger("Jump");
        }
    }

    void FixedUpdate()
    {
        //move our character
        controller.Move(horizontalMove*Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
