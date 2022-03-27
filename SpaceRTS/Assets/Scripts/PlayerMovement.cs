using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D playerBody;

    public Vector3 targetPosition;
    public Vector3 change;

    public Animator animator;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerBody.gravityScale = 0;
    }

    void Update()
    {
        change = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };
        if (change.magnitude > 0.1)
        {
            playerBody.MovePosition(transform.position + change * speed * Time.deltaTime);
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
        }
    }
}
