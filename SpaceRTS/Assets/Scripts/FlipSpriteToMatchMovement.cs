using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSpriteToMatchMovement : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
            animator.enabled = body.velocity.magnitude > 0.01f;

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
