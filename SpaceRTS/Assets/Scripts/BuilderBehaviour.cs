using UnityEngine;

using GameLogic;
using GameLogic.Entities;

public class BuilderBehaviour : MovableEntity, IBuilder
{
    [HideInInspector]
    public BuilderLogic.Action currentAction = BuilderLogic.Action.Idle;

    public override void Update() =>
        currentAction = BuilderLogic.applyBuilderAction(currentAction, this);

    public bool _selected = false;
    public bool selected { get => _selected; set => _selected = value; }

    public void setHealth(float value) => health = value;

    public void OnCollisionEnter2D(Collision2D collision) =>
        currentAction = BuilderLogic.handleCollision(currentAction, this, collision.gameObject.name);

    public void OnCollisionExit2D(Collision2D collision) =>
        currentAction = BuilderLogic.handleCollisionLeave(currentAction, this, collision.gameObject.name);

    public void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            gameState.selectAnotherBuilder(this);
        }
        else
        {
            gameState.selectBuilder(this);
        }
    }

    public void setBuildTarget(IStructure target) =>
        currentAction = BuilderLogic.Action.NewBuild(BuilderLogic.BuildState.NewMovingToBuild(target));

    public void OnGUI() =>
        BuilderLogic.displayCurrentAction(currentAction);
}
