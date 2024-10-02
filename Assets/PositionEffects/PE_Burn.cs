using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PE_Burn : PositionEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    public override void OnTurnStart(Character currentTurnChara, int turnCount)
    {
        if (character != null && character == currentTurnChara)
        {
            Action.ActionStatus action = actionStatus;
            action.decreaseHP_min = PEStatus.stack;
            action.decreaseHP_max = PEStatus.stack;
            Enqueue(action, true, new List<Character>() { character }, 0, true);
        }
    }

    public override void OnRoundEnd()
    {
        AddStack(-2);
    }

}
