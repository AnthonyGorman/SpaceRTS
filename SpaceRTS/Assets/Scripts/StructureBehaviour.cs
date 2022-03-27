using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameLogic.Entities;

public class StructureBehaviour : Entity, IStructure
{
    public float _buildSpeed;
    public float buildSpeed => _buildSpeed;

    public BuildProgress buildProgress { get; set; } = BuildProgress.Foundation;

    public void setBuildProgress(BuildProgress value) =>
        buildProgress = value;

    public void setHealth(float value) =>
        health = value;

    public override void Update() { }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var builders = gameState.getBuilders;

            gameState.dispatchBuild(this);
        }
    }
}
