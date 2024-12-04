using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Burn : PA_StatusEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn)
        {
            Action.ActionStatus action = actionStatus;
            action.decreaseHP_min = StEStatus.DMGPerTurn;
            action.decreaseHP_max = StEStatus.DMGPerTurn;
            Enqueue(action, true, new List<Character>() { character });
            if (StEStatus.applyer != null) { StEStatus.applyer.GetBattleReport().decreaseHP += StEStatus.DMGPerTurn; }
            AddStack(-1);
        }
    }
}
