using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D playerBody;

    public Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerBody.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 change = (transform.position - targetPosition).normalized;

        playerBody.MovePosition(transform.position + change * speed * Time.deltaTime);
    }
}
