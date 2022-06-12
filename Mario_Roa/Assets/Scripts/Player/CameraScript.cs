using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target; //Who is to follow
    public Transform leftBounds; // left edge of the environment
    public Transform rightBounds; // right edge of the environment

    //smoothnes of camera follow
    public float smoothDampTime = 0.15f;
    private Vector3 smoothDampVelocity = Vector3.zero;

    private float camWidth;
    private float camHeight;
    private float levelMinX;
    private float levelMaxX;

    void Start()
    {
        //Giving values to our cameras width, height. minimum and maximum area to follow

        camHeight = Camera.main.orthographicSize * 2;
        camWidth = camHeight * Camera.main.aspect;

        float leftBoundsWidth = leftBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        float rightBoundsWidth = rightBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;

        levelMinX = leftBounds.position.x + leftBoundsWidth + (camWidth / 2);
        levelMaxX = rightBounds.position.x - rightBoundsWidth - (camWidth / 2);
    }


    void Update()
    {
        //Check if Character is stantiated and create a variable for camera position to not exceed the levelmaxX or go further than levelminX
        if (target)
        {
            float targetX = Mathf.Max(levelMinX, Mathf.Min(levelMaxX, target.position.x));

            // Make a smoothing camera follow from original position to target position by specific amount of time
            float x = Mathf.SmoothDamp(transform.position.x, targetX, ref smoothDampVelocity.x, smoothDampTime);
            
            //Assigning the float x we make to our camera position
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        
        }
    }
}
