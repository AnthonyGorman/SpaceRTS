using UnityEngine;

using GameLogic;
using GameLogic.Entities;

public class BuilderBehaviour : MovableEntity, IBuilder
{
    [HideInInspector]
    public BuilderLogic.Action currentAction = BuilderLogic.Action.Idle;

    public override void Start()
    {
        base.Start();
        rigidBody.gravityScale = 0;
    }

    public override void Update() =>
        currentAction = BuilderLogic.applyBuilderAction(currentAction, this);

    public bool _selected = false;
    public bool selected { get => _selected; set => _selected = value; }

    public void setHealth(float value)
    {
        health = value;
        if (health <= 0)
        {
            Destroy(this);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) =>
        currentAction = BuilderLogic.handleCollision(currentAction, this, collision.gameObject.name);

    public void OnCollisionExit2D(Collision2D collision) =>
        currentAction = BuilderLogic.handleCollisionLeave(currentAction, this, collision.gameObject.name);

    public void OnMouseDown() =>
        BuilderLogic.handleMouseDown(this);

    public void setBuildTarget(IStructure target) =>
        currentAction = BuilderLogic.Action.NewBuild(BuilderLogic.BuildState.NewMovingToBuild(target));

    public void setRepairTarget(IStructure target) =>
        currentAction = BuilderLogic.Action.NewRepair(BuilderLogic.RepairState.NewRepairing(target));

    public void setMoveTarget(Vector3 target) =>
        currentAction = BuilderLogic.Action.NewMovingTo(target);

    public void setMoneyPickupTarget(string name, Vector3 target) =>
        currentAction = BuilderLogic.Action.NewPickupMoney(name, target);
}
