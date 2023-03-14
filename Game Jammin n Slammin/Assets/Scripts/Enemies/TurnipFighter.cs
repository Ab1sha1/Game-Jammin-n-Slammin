using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnipFighter : MonoBehaviour
{
    public EnemyStats enemyStats;

    void Update()
    {
        if(enemyStats.Health == 0) 
        {
            Die();
        }
    }

    void Die() 
    {
        Destroy(gameObject);
    }
}
