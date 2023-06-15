using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    
    [SerializeField]
    protected CharacterController2D CC2D;
    
    [SerializeField]
    protected GameObject[] m_gameObject;
    [Header("Parameters")]
    [SerializeField]
    protected float moveSpeed = 5f;
    [SerializeField]
    protected float walkSpeed = 0.2f;
    [SerializeField]
    protected float crouchSpeed = 3f;
    [SerializeField]
    [Range(0, 1000)]
    protected float jumpForce = 200;


    protected bool sword = false;
    protected bool idleSword = false;
    protected bool canAttack = false;
    protected bool isDead = false;
    private bool crouching = false;


    // Update is called once per frame
    void Update()
    {
        crouching = Input.GetKey(KeyCode.S) && CC2D.IsGrounded;
        if (isDead)
        {
            this.CC2D.m_rigidbody2D.simulated = false;
            return;
        }
        float xSpeed = crouching ? crouchSpeed : moveSpeed;
        CC2D.Move(new Vector2(Input.GetAxis("Horizontal") * xSpeed, 0f));

        if (Input.GetButtonDown("Jump") && this.CC2D.IsGrounded)
        {
            CC2D.Jump(jumpForce);
            animator.SetTrigger("Jump");
        }

        

        if (CC2D.m_rigidbody2D.velocity.x < 0 && CC2D.FacingRight)
        {
            CC2D.Flip();
        }
        else if (CC2D.m_rigidbody2D.velocity.x > 0 && !CC2D.FacingRight)
        {
            CC2D.Flip();
        }

        animator.SetBool("Crouch", crouching);
        animator.SetFloat("SpeedX", Mathf.Abs(CC2D.m_rigidbody2D.velocity.x));
        animator.SetFloat("VelocityY", CC2D.m_rigidbody2D.velocity.y);
        animator.SetBool("Grounded", CC2D.IsGrounded);
    }
    public virtual void Death()
    {
        isDead = true;
        animator.SetTrigger("Death");
    }

}

