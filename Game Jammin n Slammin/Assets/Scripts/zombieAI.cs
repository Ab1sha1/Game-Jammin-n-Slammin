using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieAI : MonoBehaviour
{
    public float moveSpeed;
    public bool mustPatrol;
    private bool mustTurn;

    public Rigidbody2D rb;
    public Transform groundCheckPos;
    public float groundCheckRange;
    public LayerMask groundLayer;

    public float damage;

    void Start()
    {
        mustPatrol = true;
    }

    void Update()
    {
        if (mustPatrol) 
        {
            Patrol();
        }
    }

    private void FixedUpdate()
    {
        mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRange, groundLayer);
    }

    void Patrol() 
    {
        if (mustTurn) 
        {
            Flip();
        }

        rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    void Flip() 
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        moveSpeed *= -1;
        mustPatrol = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRange);
    }
}
