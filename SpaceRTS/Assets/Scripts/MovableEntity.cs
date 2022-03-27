using UnityEngine;

using GameLogic.Entities;

public abstract class MovableEntity : Entity, IMovableEntity
{
    public float _speed = 1.0f;
    public float speed { get => _speed; set => _speed = value; }

    public Vector3 velocity => rigidBody.velocity;

    public void setVelocity(Vector3 value) =>
        rigidBody.velocity = value;
}
