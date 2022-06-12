using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreVisual : MonoBehaviour
{
    float lifeSpan = 2.0f;

    private void Update()
    {    //Make the objects float or going upward using translate for 2 seconds
        this.gameObject.transform.Translate(0, 2f * Time.deltaTime, 0);

        //Decrement the lifespan. minus 1 every second
        lifeSpan -= 1.0f * Time.deltaTime;

        //destroy this object when the lifespan hit 0
        if (lifeSpan < 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}

