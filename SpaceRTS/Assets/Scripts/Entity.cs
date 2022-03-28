using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameLogic.Entities;

public abstract class Entity : MonoBehaviour, IEntity
{
    public GameState gameState;
    public IGlobalState globalState => gameState;

    public float _maxHealth;
    public float maxHealth => _maxHealth;

    public float _health;
    public float health { get => _health; set => _health = value; }

    public Vector3 position => transform.position;

    public Rigidbody2D rigidBody { get; set; }
    // Start is called before the first frame update

    public virtual void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = 0;
    }

    // Update is called once per frame
    public abstract void Update();
}
