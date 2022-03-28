using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using GameLogic;

public class CanPurchase : MonoBehaviour
{
    public Button button;
    public HookedBehaviour hook;
    public short purchaseCost;

    public void Start()
    {
        button = GetComponent<Button>();
        hook = GetComponent<HookedBehaviour>();
    }

    public void Update() =>
        button.interactable = ButtonLogic.shouldButtonBeEnabled(purchaseCost, hook.gameState);
}
