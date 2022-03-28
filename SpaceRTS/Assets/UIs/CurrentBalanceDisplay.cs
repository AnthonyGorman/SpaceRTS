using UnityEngine;
using UnityEngine.UI;

public class CurrentBalanceDisplay : MonoBehaviour
{
    public HookedBehaviour hook;
    public Text text;
    public GameState gameState => hook.gameState;

    public void Start()
    {
        text = GetComponent<Text>();
        hook = GetComponent<HookedBehaviour>();
    }

    public void Update() =>
        text.text = $"Current balance: ${gameState.currentBalance}";

}
