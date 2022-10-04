using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    //https://github.com/DanielDFY/Hollow-Knight-Imitation
    //Script Created by DanielDFY

    public int health;
    public float moveSpeed;
    public float jumpSpeed;
    public int jumpLeft;
    public Vector2 climbJumpForce;
    public float fallSpeed;
    public float sprintSpeed;
    public float sprintTime;
    public float sprintInterval;
    public float attackInterval;

    public Color invulColor;
    public Vector2 hurtRecoil;
    public float hurtTime;
    public float hurtRecoverTime;
    public float deathRecoil;
    public float deathDelay;

    public Vector2 attackUpRecoil;
    public Vector2 attackForwardRecoil;
    public Vector2 attackDownRecoil;

    // Gameobject extra effects if not animated

    private bool _isGrounded;
    private bool _isClimb;
    private bool _isSprintable;
    private bool _isSprintReset;
    private bool _isInputEnabled;
    private bool _isFalling;
    private bool _isAttackable;

    private float _climbJumpDelay = 0.2f;
    private float _attackEffectLifeTime = 0.05f;

    private Animator _anim;
    private Rigidbody2D _rb;
    private Transform _transform;
    private SpriteRenderer _spriteRend;
    private BoxCollider2D _boxCollider;

    // Sorry Henny
    /*
    public float wallJumpForce;
    public float canMove = 1;
    public bool canWallJump;
    public float wallJumpDirection;
    public float wallJumpTimer;
    public float wallTime;
  //  public MeshRenderer swordMR;
    // public Animator anim;
    */

    // public AudioSource[] playerSounds;



    void Start()
    {
        _isInputEnabled = true;
        _isSprintReset = true;
        _isAttackable = true;

        _anim = gameObject.GetComponent<Animator>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _transform = gameObject.GetComponent<Transform>();
        _spriteRend = gameObject.GetComponent<SpriteRenderer>();
        _boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        updatePlayerState();
        if (_isInputEnabled)
        {
            move();
            jumpControl();
            fallControl();
            sprintControl();
            attackControl();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // enter climb state
        if (collision.collider.tag == "Walls" && !_isGrounded)
        {
            _rb.gravityScale = 0;

            Vector2 newVelocity;
            newVelocity.x = 0;
            newVelocity.y = -2;

            _rb.velocity = newVelocity;

            _isClimb = true;
            _anim.SetBool("IsClimb", true);

            _isSprintable = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Walls" && _isFalling && !_isClimb)
        {
            OnCollisionEnter2D(collision);
        }
    }

    public void hurt(int damage)
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerInvulnerable");

        health = Math.Max(health - damage, 0);

        if (health == 0)
        {
            die();
            return;
        }

        // enter invulnerable state
        _anim.SetTrigger("IsHurt");

        // stop player movement
        Vector2 newVelocity;
        newVelocity.x = 0;
        newVelocity.y = 0;
        _rb.velocity = newVelocity;

        // visual effect
        _spriteRend.color = invulColor;

        // death recoil
        Vector2 newForce;
        newForce.x = -_transform.localScale.x * hurtRecoil.x;
        newForce.y = hurtRecoil.y;
        _rb.AddForce(newForce, ForceMode2D.Impulse);

        _isInputEnabled = false;

        StartCoroutine(recoverFromHurtCoroutine());
    }

    private IEnumerator recoverFromHurtCoroutine()
    {
        yield return new WaitForSeconds(hurtTime);
        _isInputEnabled = true;
        yield return new WaitForSeconds(hurtRecoverTime);
        _spriteRend.color = Color.white;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // exit climb state
        if (collision.collider.tag == "Walls")
        {
            _isClimb = false;
            _anim.SetBool("IsClimb", false);

            _rb.gravityScale = 1;
        }
    }

    /* ######################################################### */

    private void updatePlayerState()
    {
        _isGrounded = checkGrounded();
        _anim.SetBool("IsGround", _isGrounded);

        float verticalVelocity = _rb.velocity.y;
        _anim.SetBool("IsDown", verticalVelocity < 0);

        if (_isGrounded && verticalVelocity == 0)
        {
            _anim.SetBool("IsJump", false);
            _anim.ResetTrigger("IsJumpFirst");
            _anim.ResetTrigger("IsJumpSecond");
            _anim.SetBool("IsDown", false);

            jumpLeft = 1;
            _isClimb = false;
            _isSprintable = true;
        }
        else if (_isClimb)
        {
            // one remaining jump chance after climbing
            jumpLeft = 1;
        }
    }

    private void move()
    {
        // calculate movement
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;

        // set velocity
        Vector2 newVelocity;
        newVelocity.x = horizontalMovement;
        newVelocity.y = _rb.velocity.y;
        _rb.velocity = newVelocity;

        if (!_isClimb)
        {
            // the sprite itself is NOT inversed 
            float moveDirection = transform.localScale.x * horizontalMovement;

            if (moveDirection < 0)
            {
                // flip player sprite
                Vector3 newScale;
                newScale.x = horizontalMovement < 0 ? -1 : 1;
                newScale.y = 1;
                newScale.z = 1;

                transform.localScale = newScale;

                if (_isGrounded)
                {
                    // turn back animation
                    _anim.SetTrigger("IsRotate");
                }
            }
            else if (moveDirection > 0)
            {
                // move forward
                _anim.SetBool("IsRun", true);
            }
        }

        // stop
        if (Input.GetAxis("Horizontal") == 0)
        {
            _anim.SetTrigger("stopTrigger");
            _anim.ResetTrigger("IsRotate");
            _anim.SetBool("IsRun", false);
        }
        else
        {
            _anim.ResetTrigger("stopTrigger");
        }
    }

    private void jumpControl()
    {
        if (!Input.GetButtonDown("Jump"))
            return;

        if (_isClimb)
            climbJump();
        else if (jumpLeft > 0)
            jump();
    }

    private void fallControl()
    {
        if (Input.GetButtonUp("Jump") && !_isClimb)
        {
            _isFalling = true;
            fall();
        }
        else
        {
            _isFalling = false;
        }
    }

    private void sprintControl()
    {
        if (Input.GetKeyDown(KeyCode.K) && _isSprintable && _isSprintReset)
            sprint();
    }

    private void attackControl()
    {
        if (Input.GetKeyDown(KeyCode.J) && !_isClimb && _isAttackable)
            attack();
    }

    private void die()
    {
        _anim.SetTrigger("IsDead");

        _isInputEnabled = false;

        // stop player movement
        Vector2 newVelocity;
        newVelocity.x = 0;
        newVelocity.y = 0;
        _rb.velocity = newVelocity;

        // visual effect
        _spriteRend.color = invulColor;

        // death recoil
      //  Vector2 newForce;
       // newForce.x = -_transform.localScale.x * deathRecoil.x;
       // newForce.y = deathRecoil.y;
      //  _rb.AddForce(newForce, ForceMode2D.Impulse);

        StartCoroutine(deathCoroutine());
    }

    private IEnumerator deathCoroutine()
    {
        var material = _boxCollider.sharedMaterial;
        material.bounciness = 0.3f;
        material.friction = 0.3f;
        // unity bug, need to disable and then enable to make it work
        _boxCollider.enabled = false;
        _boxCollider.enabled = true;

        yield return new WaitForSeconds(deathDelay);

        material.bounciness = 0;
        material.friction = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /* ######################################################### */

    private bool checkGrounded()
    {
        Vector2 origin = _transform.position;

        float radius = 0.2f;

        // detect downwards
        Vector2 direction;
        direction.x = 0;
        direction.y = -1;

        float distance = 1f;
        LayerMask layerMask = LayerMask.GetMask("Ground");

        RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        return hitRec.collider != null;
      
    }
    private void OnDrawGizmos()
    {
        Vector2 origin = this.transform.position;
        Vector2 direction;
        direction.x = 0;
        direction.y = -1;

        float distance = 1f;

        float radius = 0.2f;
        Gizmos.DrawWireSphere(origin + (direction * distance), radius);
    }

    private void jump()
    {
        Vector2 newVelocity;
        newVelocity.x = _rb.velocity.x;
        newVelocity.y = jumpSpeed;

        _rb.velocity = newVelocity;

        _anim.SetBool("IsJump", true);
        jumpLeft -= 1;
        if (jumpLeft == 0)
        {
            _anim.SetTrigger("IsJumpSecond");
        }
        else if (jumpLeft == 1)
        {
            _anim.SetTrigger("IsJumpFirst");
        }
    }

    private void climbJump()
    {
        Vector2 realClimbJumpForce;
        realClimbJumpForce.x = climbJumpForce.x * transform.localScale.x;
        realClimbJumpForce.y = climbJumpForce.y;
        _rb.AddForce(realClimbJumpForce, ForceMode2D.Impulse);

        _anim.SetTrigger("IsClimbJump");
        _anim.SetTrigger("IsJumpFirst");

        _isInputEnabled = false;
        StartCoroutine(climbJumpCoroutine(_climbJumpDelay));
    }

    private IEnumerator climbJumpCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        _isInputEnabled = true;

        _anim.ResetTrigger("IsClimbJump");

        // jump to the opposite direction
        Vector3 newScale;
        newScale.x = -transform.localScale.x;
        newScale.y = 1;
        newScale.z = 1;

        transform.localScale = newScale;
    }

    private void fall()
    {
        Vector2 newVelocity;
        newVelocity.x = _rb.velocity.x;
        newVelocity.y = -fallSpeed;

        _rb.velocity = newVelocity;
    }

    private void sprint()
    {
        // reject input during sprinting
        _isInputEnabled = false;
        _isSprintable = false;
        _isSprintReset = false;

        Vector2 newVelocity;
        newVelocity.x = transform.localScale.x * (_isClimb ? sprintSpeed : -sprintSpeed);
        newVelocity.y = 0;

        _rb.velocity = newVelocity;

        if (_isClimb)
        {
            // sprint to the opposite direction
            Vector3 newScale;
            newScale.x = -transform.localScale.x;
            newScale.y = 1;
            newScale.z = 1;

            transform.localScale = newScale;
        }

        _anim.SetTrigger("IsSprint");
        StartCoroutine(sprintCoroutine(sprintTime, sprintInterval));
    }

    private IEnumerator sprintCoroutine(float sprintDelay, float sprintInterval)
    {
        yield return new WaitForSeconds(sprintDelay);
        _isInputEnabled = true;
        _isSprintable = true;

        yield return new WaitForSeconds(sprintInterval);
        _isSprintReset = true;
    }

    private void attack()
    {
        float verticalDirection = Input.GetAxis("Vertical");
        if (verticalDirection > 0)
            attackUp();
        else if (verticalDirection < 0 && !_isGrounded)
            attackDown();
        else
            attackForward();
    }
    
    private void attackUp()
    {
        _anim.SetTrigger("IsAttackUp");
        //attackUpEffect.SetActive(true);

        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = 1;

        StartCoroutine(attackCoroutine( _attackEffectLifeTime, attackInterval, detectDirection, attackUpRecoil));
    }
    
    private void attackForward()
    {
        _anim.SetTrigger("IsAttack");
       // attackForwardEffect.SetActive(true);

        Vector2 detectDirection;
        detectDirection.x = -transform.localScale.x;
        detectDirection.y = 0;

        Vector2 recoil;
        recoil.x = transform.localScale.x > 0 ? -attackForwardRecoil.x : attackForwardRecoil.x;
        recoil.y = attackForwardRecoil.y;

        StartCoroutine(attackCoroutine( _attackEffectLifeTime, attackInterval, detectDirection, recoil));
    }

    private void attackDown()
    {
        _anim.SetTrigger("IsAttackDown");
       // attackDownEffect.SetActive(true);

        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = -1;

        StartCoroutine(attackCoroutine( _attackEffectLifeTime, attackInterval, detectDirection, attackDownRecoil));
    }

    private IEnumerator attackCoroutine(float effectDelay, float attackInterval, Vector2 detectDirection, Vector2 attackRecoil)
    {
        Vector2 origin = _transform.position;

        float radius = 0.6f;

        float distance = 1.5f;
        LayerMask layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Trap") | LayerMask.GetMask("Switch") | LayerMask.GetMask("Projectile");

        RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(origin, radius, detectDirection, distance, layerMask);

        foreach (RaycastHit2D hitRec in hitRecList)
        {
            GameObject obj = hitRec.collider.gameObject;

            string layerName = LayerMask.LayerToName(obj.layer);

            if (layerName == "Switch")
            {
             //   Switch swithComponent = obj.GetComponent<Switch>();
            //    if (swithComponent != null)
            //        swithComponent.turnOn();
            }
            else if (layerName == "Enemy")
            {
            //    EnemyController enemyController = obj.GetComponent<EnemyController>();
             //   if (enemyController != null)
            //        enemyController.hurt(1);
            }
            else if (layerName == "Projectile")
            {
                Destroy(obj);
            }
        }

        if (hitRecList.Length > 0)
        {
            _rb.velocity = attackRecoil;
        }

        yield return new WaitForSeconds(effectDelay);

     //   attackEffect.SetActive(false);

        // attack cool down
        _isAttackable = false;
        yield return new WaitForSeconds(attackInterval);
        _isAttackable = true;
    }
}

      
    



    //Scrap Box DONT DELETE YET

   // private void OnCollisionStay2D(Collision2D collision)
    //{
        //  if (collision.gameObject.tag == "Walls" && Input.GetKeyDown(KeyCode.Space))
        //  {
        //     
        //     print("jerry2");
        //     rb.velocity = Vector2.left * jumpHeight;
        // }
  //  }

    
    /*
      
     private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (wallJumpTimer > wallTime)
        {
            moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed * canMove, rb.velocity.y);
        }


        if (facingRight == false && moveInput > 0 && canWallJump == false)
        {
           // Flip();
        }
        else if (facingRight == true && moveInput < 0 && canWallJump == false)
        {
           // Flip();
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



     private void OnCollisionEnter2D(Collision2D collision)
    {

       
        if (collision.gameObject.tag == "Walls" && isGrounded == false)
        {
            
            canMove = 0;
            canWallJump = true;
            rb.velocity = rb.velocity * 0;
            //Flip();

        }
    }
     
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


        if(Input.GetKeyDown("z"))
        {
            swordMR.enabled = true;
            
        }
        if(Input.GetKeyUp("z"))
        {
            swordMR.enabled = false;
            
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

     */


