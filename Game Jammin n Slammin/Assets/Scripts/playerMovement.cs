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


    public float wallJumpForce;
    public float canMove = 1;
    public bool canWallJump;
    public float wallJumpDirection;
    public float wallJumpTimer;
    public float wallTime;
    // public Animator anim;


    // public AudioSource[] playerSounds;

    public Vector2 latevelocity;

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
        if (facingRight == true)
        {
            wallJumpDirection = 1;
        }
        else
        {
            wallJumpDirection = -1;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);




        if (facingRight == false && moveInput > 0 && canWallJump == false)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0 && canWallJump == false)
        {
            Flip();
        }
        if (wallJumpTimer > wallTime)
        {
            moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * speed * canMove, rb.velocity.y);
        }

        if (canWallJump == true && Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector2(wallJumpForce * wallJumpDirection, jumpHeight);
            //  rb.AddForce(new Vector2(20 * wallJumpDirection, 3));
            canWallJump = false;
            canMove = 1;
            wallJumpTimer = 0;
        }
        wallJumpTimer = wallJumpTimer + Time.deltaTime;
    }

    public void LateUpdate()
    {
        // latevelocity = rb.velocity.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "Walls" && isGrounded == false)
        {
            print("jerry");
            canMove = 0;
            canWallJump = true;
            rb.velocity = rb.velocity * 0;
            Flip();

        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0f, 180, 0f);
        if(wallJumpTimer > 0) 
        {
            wallJumpTimer = 5;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //  if (collision.gameObject.tag == "Walls" && Input.GetKeyDown(KeyCode.Space))
        //  {
        //     
        //     print("jerry2");
        //     rb.velocity = Vector2.left * jumpHeight;
        // }
    }


}

