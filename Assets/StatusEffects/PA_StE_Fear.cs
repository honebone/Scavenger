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
    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn)
        {
            Enqueue_Self(actionStatus);
            AddStack(-1);
        }
    }

    public override void AtTheEnd()
    {
        character.AddExDMG_Mul(-exDMG);
    }
}
