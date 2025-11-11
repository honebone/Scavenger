using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Charm : PA_StatusEffect
{
    [SerializeField] Action.ActionStatus actionStatus;
    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn && applyFlag)
        {
            AddStack(-1);
        }
    }
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn)
        {
            Enqueue_Self(actionStatus);
        }
    }
}
