using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject scoreVisualPrefab;
    //For Enemys Facing Direction
    private float dirX;

    //Enemy move speed value
    public float moveSpeed;


    public Rigidbody2D rb;

    //Check if enemt is facing right
    public bool facingRight = false;

    //Hold the value of or players scale
    private Vector3 localScale;

    public bool isDead = false;

    //Variable for GameManager Object
    private GameObject gm;

    //Variable for audio source

    public AudioSource source;

    public AudioClip enemyDeathClip;

    void Start()
    {
        // Giving a value to our variables
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 3f;

        // disable the scripts at the start we need to enable the scripts if the enemy is in the players camera view for performance
        enabled = false;
    }

    //Check if the enemy collides in the wall 
    private void OnTriggerEnter2D(Collider2D col)
    {
        //if its collide in the wall, walk in opposite direction
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Enemy")
        {
            dirX *= -1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            dirX *= -1f;
        }
    }

    //Enemy movement script
    void FixedUpdate()
    {
        if(isDead == false)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }
    }

    //Call CheckWhereToFace function in lateupdate
    private void LateUpdate()
    {
        CheckWhereToFace();
    }

    //check where the enemy is facing base on direction and adjust the scale to make of illusion that is flipping
    void CheckWhereToFace()
    {
        if(dirX > 0)
        {
            facingRight = true;
        }
        else if (dirX < 0)
        {
            facingRight = false;
        }

        if(((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))        
            localScale.x *= -1f;

        transform.localScale = localScale;
    }

    //Enable the scripts when its on players camera view
    private void OnBecameVisible()
    {
        enabled = true;
    }

    //Turn off the script if this enemy exit the player view
    private void OnBecameInvisible()
    {
        if(isDead == false)
        {
            enabled = false;
        }
    }

    //Spawn a Visual Coin at Enemys position to visualize that player gain coins or score from killing the enemy
    public void SpawnScoreSpawnVisual()
    {
        GameObject a = Instantiate(scoreVisualPrefab, this.gameObject.transform.position, Quaternion.identity) as GameObject;

        gm = GameObject.FindWithTag("GameManager");
        gm.GetComponent<GameManager>().score += 100;
    }

    public void Died()
    {
        //Change the this enemy tag to Dead
        this.gameObject.tag = "Dead";
        isDead = true;

        //Play Enemy Death Sound

        source.PlayOneShot(enemyDeathClip);

        //Make the enemy jump when died before falling
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10), ForceMode2D.Impulse);

        //enable trigger in colliders for the character to fall
        GetComponent<BoxCollider2D>().isTrigger = true;

        SpawnScoreSpawnVisual();

        //Call a Destroy function
        Invoke("ThisEnemyIsDead", 2);
    }


    //functions to call when this enemy died
    public void ThisEnemyIsDead()
    {
        Destroy(this.gameObject);
    }

}
