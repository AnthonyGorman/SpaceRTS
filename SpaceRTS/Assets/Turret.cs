using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLogic.Entities;

public class Turret : MonoBehaviour
{
    IEntityWithHealth? target;
    public Transform turretPrefab;
    public int cooldown = 0;
    public int cooldownTime = 200;
    private int nextHit = 0;
    public float _attackStrength;
    public float attackStrength { get => _attackStrength; set => _attackStrength = value; }
    private void Start()
    {
       
    }
    void Update()
    {
        if (target != null)
        {
            if (cooldown == nextHit)
            {
                target.setHealth(target.health - attackStrength);
                cooldown = cooldownTime;
                Debug.Log("hit");
            }
            cooldown--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Brute")
        {
            var objectHealth = collision.gameObject.GetComponent<IEntityWithHealth>();
            target = objectHealth;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        target = null;
    }


    // Update is called once per frame

}
