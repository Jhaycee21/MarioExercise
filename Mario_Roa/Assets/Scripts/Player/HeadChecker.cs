using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadChecker : MonoBehaviour
{

    //Variable for AudioSource

    public AudioSource source;

    public AudioClip hitBrick;

    //Check if Head Collide in Question Block
    private void OnCollisionEnter2D(Collision2D cx)
    {
        if (cx.gameObject.tag == "QuestionBlock")
        {
            //Get the HitByPlayer function of the block that collided with
            cx.gameObject.GetComponent<QuestionBlock>().HitByPlayer();
        }


        if (cx.gameObject.tag == "Brick")
        {
            //Play Hit Brick Audio
            source.PlayOneShot(hitBrick);

            //Destroy Brick when collide with head
            Destroy(cx.gameObject);
        }

    }
}
