using UnityEngine;

using GameLogic;

public class PositionManagement : MonoBehaviour
{
    public HookedBehaviour hook;
    public GameState gameState => hook.gameState;

    private void Start() =>
        hook = GetComponent<HookedBehaviour>();

    public void Update() =>
        GameScale.handleUpdate(gameState);
}
