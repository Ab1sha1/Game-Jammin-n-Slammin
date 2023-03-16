using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnipFighter : MonoBehaviour
{
    EnemyStats enemyStats;

    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
    }
    void Update()
    {
        if(enemyStats.Health == 0) 
        {
            Die();
        }
    }

    void Die() 
    {
        //play death animation
        Destroy(gameObject);
    }
}
