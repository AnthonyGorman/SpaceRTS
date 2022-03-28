using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameLogic.Entities;

public class EnemyAI : MovableEntity, IEnemy
{
    IEntityWithHealth? attackTarget = null;
    bool isMoving;
    bool isHurt;

    public float _attackRange;
    public float attackRange { get => _attackRange; set => _attackRange = value; }
    public float _attackStrength;
    public float attackStrength { get => _attackStrength; set => _attackStrength =value; }

    [SerializeField]
    public IStructure spaceship;

    public int cooldown = 0;
    public int cooldownTime = 200;
    private int nextHit = 0;
   
    // Update is called once per frame
    public override void Update()
    {
        
        if (isMoving)
        {
            var targetVelocity = (spaceship.position - position).normalized * speed;

            //Debug.Log($"Moving {targetVelocity.x}, {targetVelocity.y}");
            setVelocity(targetVelocity);
        }

        if (attackTarget != null)
        {
            if (cooldown == nextHit)
            {
                attackTarget.setHealth(attackTarget.health - attackStrength);
                cooldown = cooldownTime;
                Debug.Log("hit");
            }
            cooldown--;
        }
        
        
    }

    public override void Start()
    {
        base.Start();
        attackTarget = null;
        isMoving = true;
        isHurt = false;

        spaceship = GameObject.Find("SpaceShip").GetComponent<StructureBehaviour>();
        if (spaceship is null)
        {
            Debug.LogError("Could not get spaceship!");
        }
    }

    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "SpaceShip")
        {
            var objectHealth = collision.gameObject.GetComponent<IEntityWithHealth>();
            attackTarget = objectHealth;
            isMoving = false;
            setVelocity(new Vector3());
            
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        isMoving = true;
        attackTarget = null;
    }

    void Move()
    {

    }

    public void setHealth(float value) =>
        health = value;
}
