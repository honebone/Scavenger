using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StE_Irritated : PA_StatusEffect
{
    [SerializeField] int SANDMGPerStack;
    [SerializeField] Action.ActionStatus actionStatus;

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn)
        {
            Action.ActionStatus action = actionStatus;
            action.SANDamage_min = StEStatus.stack * SANDMGPerStack;
            action.SANDamage_max = StEStatus.stack * SANDMGPerStack;
            Enqueue_Self(action);
        }
    }
}
