using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PE_Regeneration : PositionEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    public override void OnTurnStart(Character currentTurnChara, int turnCount)
    {
        if (character != null && character == currentTurnChara)
        {
            Action.ActionStatus action = actionStatus;
            action.trueHeal = PEStatus.DMGPerTurn;
           Enqueue(action, true, new List<Character>() { character },0, true);
        }
    }

    public override void OnRoundEnd()
    {
        AddStack(-1);
    }
}
