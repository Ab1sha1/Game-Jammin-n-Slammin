using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

namespace Luke
{
    [RequireComponent(typeof(Controller))]
    public class WeaponHandler : MonoBehaviour
    {
        private Controller _controller;
        private Rigidbody2D _body;
        private Animator _anim;

        public int _weaponIndex;

        public LayerMask enemyLayers;

        public Transform swordPoint;
        public int swordDamage;
        public float swordRange;


        public int lowBowDamage;
        public int MaxBowDamage;

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            _controller = GetComponent<Controller>();
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
            
            }

            if (Input.GetMouseButtonDown(0) && _weaponIndex == 0) 
            {
                SwordAttack();
            }

        }

        void SwordAttack() 
        {
            //Play animation
            //Run Hit Check
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordPoint.position, swordRange, enemyLayers);
            //Grab the Enemies' Health and subtract damage if enemy
            foreach(Collider2D enemy in hitEnemies) 
            {
                Debug.Log("We Hit " + enemy.name);
                enemy.GetComponent<EnemyStats>().Health -= swordDamage;
            }
            //Grab the normal of the surface and bounce off it otherwise
        }

        private void OnDrawGizmosSelected()
        {
            if (swordPoint == null) 
            {
                return;
            }

            Gizmos.DrawWireSphere(swordPoint.position, swordRange); 
        }

        void BowAttack()
        {

        }
    }
}