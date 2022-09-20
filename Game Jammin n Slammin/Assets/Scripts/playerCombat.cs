using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombat : MonoBehaviour
{

    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float knockbackForce;

    public int health;

    

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            SwordAttack();
        }
    }

    void SwordAttack()
    {
      
        //Play an Attack animation


        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach(Collider2D enemy in hitEnemies) 
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockbackForce, knockbackForce));
        }

    }
    void GotHit()
    {
        health -= 1;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) 
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
