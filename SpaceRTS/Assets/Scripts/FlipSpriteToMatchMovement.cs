using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSpriteToMatchMovement : MonoBehaviour
{
    Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (body.velocity.x < 0)
        {
            // set to flip
        }
        else
        {
            // unflip
        }
    }
}
