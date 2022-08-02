using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    public float jumpHeight;

    private float moveInput;

    Rigidbody2D rb;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private bool isJumping;
    private float jumpTimeCounter;
    public float jumpTime;

    public float health;

   // public Animator anim;



   // public AudioSource[] playerSounds;

    void Start()
    {
       // anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
           // playerSounds[0].Play();
           // anim.SetTrigger("takeOff");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpHeight;
        }
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpHeight;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;

            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            //playerSounds[1].Play();
        }
        if (isGrounded == true)
        {
           // anim.SetBool("isJumping", false);
        }
        else
        {
            //anim.SetBool("isJumping", true);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy") 
        {
            
            GotHit();
        }
    }

    void GotHit() 
    {
        health -= 1f; 
    }

    void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0f, 180, 0f);
    }
}
