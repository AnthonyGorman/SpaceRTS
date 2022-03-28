using UnityEngine;

public class HookedBehaviour : MonoBehaviour
{
    public GameState gameState { get; private set; }

    public void Start() => gameState = GameObject.Find("GameState").GetComponent<GameState>();
}
