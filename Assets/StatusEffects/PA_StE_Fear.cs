using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Fear : PA_StatusEffect
{
    [SerializeField] float exDMG;
    [SerializeField] Action.ActionStatus actionStatus;
    public override void OnPAInit()
    {
        character.AddExDMG_Mul(exDMG);
    }
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

    public override void AtTheEnd()
    {
        character.AddExDMG_Mul(-exDMG);
    }
}
