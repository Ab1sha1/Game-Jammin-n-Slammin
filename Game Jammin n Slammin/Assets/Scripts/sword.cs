using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    public Rigidbody2D swordrb;
    public Rigidbody2D rbPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Walls")
        {
            rbPlayer.AddForce(transform.forward * 15);
            print("jeryy3541");
        }
    }
}
