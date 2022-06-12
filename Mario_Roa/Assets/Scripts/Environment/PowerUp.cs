using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Rigidbody2D rb;
    //For Enemys Facing Direction
    private float dirX;

    //Enemy move speed value
    public float moveSpeed;

    bool canWalk = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 4f;

        //Call the function with delay
        Invoke("StartWalking", 1);
    }

    public void StartWalking()
    {
        canWalk = true;
    }


    //Movement
    void Update()
    {   
        //Start movement only if the canwalk bool is true
        if (canWalk)
        {
            rb.velocity = new Vector2(-dirX * moveSpeed, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //if its collide in the wall, walk in opposite direction
        if (col.gameObject.tag == "Wall")
        {
            dirX *= -1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D cxx)
    {
        if(cxx.gameObject.tag == "Edge" || cxx.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
