using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameLogic;
using GameLogic.Entities;

public class StructureBehaviour : Entity, IStructure
{
    public float _buildSpeed;
    public float buildSpeed => _buildSpeed;

    public BuildProgress buildProgress { get; set; } = BuildProgress.Foundation;

    public void setBuildProgress(BuildProgress value) =>
        buildProgress = value;

    public void setHealth(float value)
    {
        health = value;
        if (health <= 0)
        {
            Destroy(this);
            if (name == "SpaceShip")
            {
                Console.WriteLine("You lose!");
                Application.Quit();
            }
        }
    }

    public override void Update() { }

    public void OnMouseOver() => StructureLogic.handleMouseOver(this);
}
