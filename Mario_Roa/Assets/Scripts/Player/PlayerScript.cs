using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    //variable for Player Spriterenderer
    public SpriteRenderer sr;

    //Players Rigidbody
    public Rigidbody2D rb;

    //Layer for the objects that our player is able to jump
    public LayerMask groundLayer;

    // holds the value how high our player can jump
    public float jumpForce;

    private float horizontal;

    private float speed = 5f;

    private bool isFacingRight = true;

    //bool used to check if player got a power ups
    public bool inPowerMode;

    //duration time of power up
    float powerUpDuration = 4f;

    // reference the GroundChecker.cs
    public GroundChecker gc;

    //Reference for Created Input Actions
    private PlayerInputAction pia;

    private GameObject gm;


    //AuidioSource Variable
    public AudioSource source;

    public AudioClip jumpClip;

    public AudioClip powerModeClip;

    public AudioClip deathClip;

    private void Awake()
    {
        pia = new PlayerInputAction();
    }

    private void OnEnable()
    {
        pia.Enable();
    }

    
    private void OnDisable()
    {
        pia.Disable();
    }
    

    void Update()
    {
        //Check when to call the Flip functions depends on where the player is facing

        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }

        UpdateAnimationStates();

        //Check inPowerMode is true to start playing the decreasing of powerup duration time
        if (inPowerMode)
        {
            DecreasePowerUp();
        }
    }

    //Movement
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    //Check Player State to apply correct animations

    void DecreasePowerUp()
    {
        powerUpDuration -= 1 * Time.deltaTime;

        if(powerUpDuration < 0)
        {
            sr.GetComponent<SpriteRenderer>().color = Color.white;
            inPowerMode = false;
            powerUpDuration = 4f;
        }
    }

    void UpdateAnimationStates()
    {
        //if player is not walking and can jump, means not jumping then idle animation will play

        if (rb.velocity.magnitude == 0 && gc.canJump == true)
        {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", false);
        }

        //if player cannot jump because its already jumping then it will play jumping animation

        if (gc.canJump == false)
        {
            GetComponent<Animator>().SetBool("isJumping", true);
            GetComponent<Animator>().SetBool("isRunning", false);
        }
        //if player is moving also can jump because its not jumping then walking or running animation will play

        if(rb.velocity.magnitude > 0 && gc.canJump == true)
        {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", true);
        }
    }

    //Jump Action that we will drag in Players Input to be called. getting rigidbody and adding a certain amount of force
    public void Jump(InputAction.CallbackContext context)
    {
        // calling the boolean in Ground checker script to check if the player can jump again once it hit the ground
        if(gc.canJump)
        {
            source.PlayOneShot(jumpClip);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        
    }

    // Making the Sprite flip or face left or right
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    //Move Action that we will drag in Players Input to be called
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    //Make the player die when collided in the enemy
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            if(inPowerMode == false)
            {
                //Make the Player jump before dying and falling
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 50), ForceMode2D.Impulse);

                //Play Death Audio
                source.PlayOneShot(deathClip);

                //enable trigger in colliders for the character to fall
                GetComponent<BoxCollider2D>().isTrigger = true;
                transform.GetChild(0).GetComponent<BoxCollider2D>().isTrigger = true;
                transform.GetChild(1).GetComponent<BoxCollider2D>().isTrigger = true;
                //Call ShowGameOver function after 2 seconds
                Invoke("ShowGameOver", 2f);
            }
            else
            {
                col.gameObject.GetComponent<Enemy>().Died();
            }

        }

        //Show the win panel when player collides in the castle
        if (col.gameObject.tag == "Castle")
        {
            //Look for GameManager.cs using tag and call a function on it
            gm = GameObject.FindWithTag("GameManager");
            gm.GetComponent<GameManager>().Win();
        }

        //Check if player get a power ups
        if(col.gameObject.tag == "PowerUp1")
        {
            //Play Power Mode Audio
            source.PlayOneShot(powerModeClip);

            //Make the player sprite turn red to visualize power ups
            sr.GetComponent<SpriteRenderer>().color = Color.red;

            inPowerMode = true;

            //Look for GameManager.cs and Add 200 score points 
            gm = GameObject.FindWithTag("GameManager");
            gm.GetComponent<GameManager>().score += 200;

            //Destroy the power up objects that collide in our player
            Destroy(col.gameObject);
        }
;
    }


    private void OnTriggerEnter2D(Collider2D cn)
    {
        if(cn.gameObject.tag == "InstantDeath")
        {
            Invoke("ShowGameOver", 2f);
        }
    }

    //Function where What Happened after the player Die
    public void ShowGameOver()
    {
        //Look for GameManager.cs using tag and call a function on it
        gm = GameObject.FindWithTag("GameManager");
        gm.GetComponent<GameManager>().Lose();
    }



}