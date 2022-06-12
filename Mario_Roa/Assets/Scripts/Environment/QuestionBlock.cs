using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    //Variable to store current position
    public Vector3 currentPosition;

    //variable to store countdown start time
    public float timeToRestoreInOriginalPosition = 0.5f;

    bool isHit = false;

    public int whichToSpawn;
    public int howManyCoins;
    public int randomCoins;

    //Variable to store coin prefab
    public GameObject Coins;

    //Variable to Spawn PowerUp Prefab
    public GameObject PowerUp;

    //Offset for Objects instantiate
    public Vector3 offSet;

    //store current sprite of this object
    public SpriteRenderer sr;

    //Store a deadblock Sprite image 
    public Sprite deadBlockNewSprite;

    //Variable for GameManager Object
    private GameObject gm;

    //Variable for AudioSource
    public AudioSource source;
    public AudioClip coinClip;
    public AudioClip powerUpClip;

    void Start()
    {
        //Choose What to Spawn
        //0 = PowerUp
        //1 = Coin
        whichToSpawn = Random.Range(0, 3);

        if (whichToSpawn == 0)
        {
            Debug.Log("PowerUp");
            howManyCoins = 1;

        }
        else
        {
            Debug.Log("Coin");
            //Generate randomly on how many coins can be generated at 1 block

            randomCoins = Random.Range(1, 5);
            howManyCoins = randomCoins;
        }

        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }


    public void HitByPlayer()
    {
        //Check if it can spawn something if its not 0
        if (howManyCoins > 0)
        {
            //Spawn Something base on random results in start()

            if (whichToSpawn == 0)
            {
                //Spawn PowerUp prefabs in this block position with offset
                GameObject a = Instantiate(PowerUp, this.gameObject.transform.position + offSet, Quaternion.identity) as GameObject;
                //Play PowerUp Audio
                source.PlayOneShot(powerUpClip);
            }
            else
            {
                //Spawn Coin prefabs in this block position with offset
                GameObject b = Instantiate(Coins, this.gameObject.transform.position + offSet, Quaternion.identity) as GameObject;

                //Play Coin Audio
                source.PlayOneShot(coinClip);

                //Find GameManager.cs to add score, along with the coin spawned
                gm = GameObject.FindWithTag("GameManager");
                gm.GetComponent<GameManager>().score += 100;
            }
        }
        else
        {
            return;
        }

        isHit = true;

        //Save the current position of this block
        currentPosition = this.transform.position;
    }

    private void Update()
    {
        if (isHit)
        {
            //translate in y axis this block
            this.gameObject.transform.Translate(0, 1f * Time.deltaTime, 0);

            //countdown for going this block to its original position
            timeToRestoreInOriginalPosition -= 1 * Time.deltaTime;

            //Condition if the countdown end this block will go back to its original position
            if(timeToRestoreInOriginalPosition < 0)
            {
                this.transform.position = currentPosition;

                //bool became false so it won't run again unless it hit by a player again
                isHit = false;

                //
                howManyCoins -= 1;

                if (howManyCoins == 0)
                {
                    this.gameObject.tag = "Dead";
                    Destroy(this.GetComponent<Animator>());
                    sr.GetComponent<SpriteRenderer>().sprite = deadBlockNewSprite;
                    Destroy(this);
                }

                //Reset the countdown of going back to original position
                timeToRestoreInOriginalPosition = 0.5f;
            }
        }
    }
}
