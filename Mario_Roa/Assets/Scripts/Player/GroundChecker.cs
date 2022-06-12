using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    //Scripts to check what the players feet is touching

    //Reference to PlayerScript.cs
    public PlayerScript ps;


    public bool canJump;

    //Check if players feet is colliding to what object or not
    private void OnCollisionStay2D(Collision2D col)
    {
        //Layer 6 is ground
        // if players feet is colliding in the ground then it can jump
        if (col.gameObject.layer == 6 || col.gameObject.tag == "QuestionBlock" || col.gameObject.tag == "Dead" || col.gameObject.tag == "Brick")
        {
            Debug.Log("Colliding In The Floor");
            canJump = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D co)
    {
        if(co.gameObject.tag == "Enemy")
        {
            co.gameObject.GetComponent<Enemy>().Died();
        }
    }


    //Check if players feet exit a collision
    private void OnCollisionExit2D(Collision2D ca)
    {
        //if player is not colliding to a ground means its already in the air jumping. and it cannot jump again or perform a multiple jump at once
        if (ca.gameObject.layer == 6 || ca.gameObject.tag == "QuestionBlock" || ca.gameObject.tag == "Dead" || ca.gameObject.tag == "Brick")
        {
            Debug.Log("Removing In The Floor");
            canJump = false;
        }
    }

}
