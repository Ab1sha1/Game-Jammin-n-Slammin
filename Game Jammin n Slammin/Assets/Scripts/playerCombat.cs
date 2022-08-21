using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombat : MonoBehaviour
{

    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    void Start()
    {

    }


    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            Attack();
        }
    }

    void Attack()
    {
        //Play an Attack animation


        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach(Collider2D enemy in hitEnemies) 
        {
            Debug.Log("We hit" + enemy.name);
        }

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
