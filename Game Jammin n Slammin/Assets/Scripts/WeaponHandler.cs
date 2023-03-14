using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
    [RequireComponent(typeof(Controller))]
    public class WeaponHandler : MonoBehaviour
    {
        private Controller _controller;
        private Rigidbody2D _body;
        private Animator _anim;

        public int swordDamage;
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
            
        }

        void SwordAttack() 
        {
            //Play animation
            //Run Hit Check
            //Grab the Enemies' Health and subtract damage
        }

        void BowAttack()
        {

        }
    }
}