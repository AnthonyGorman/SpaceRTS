using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameLogic;

public class MoneyBehaviour : MonoBehaviour
{
    HookedBehaviour hook;
    Rigidbody2D rigidBody;

    public void Start()
    {
        hook = GetComponent<HookedBehaviour>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // public void OnMouseOver() => 
        // MoneyLogic.handleMouseOver(name, rigidBody.position, hook.gameState);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("Spaceman"))
        {
            MoneyLogic.handleCollision(this, hook.gameState);
        }
    }

}
