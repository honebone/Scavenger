using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Charm : PA_StatusEffect
{
    [SerializeField] Action.ActionStatus actionStatus;
    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn && applyFlag)
        {
            Enqueue_Self(actionStatus);
            AddStack(-1);
        }
    }
}
