using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_A : MonoBehaviour
{

    public Rigidbody2D body;
    public float speed = 5f;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");


        //if (Mathf.Abs(xInput) > 0)
        //{
        //    body.linearVelocity = new Vector2(xInput * speed, body.linearVelocity.y);
        //}

        //if (Mathf.Abs(yInput) > 0)
        //{
        //    body.linearVelocity = new Vector2( body.linearVelocity.x, yInput * speed);

        //}

        Vector2 direction= new Vector2(xInput, yInput).normalized;

        body.linearVelocity = direction * speed;
    }
}
