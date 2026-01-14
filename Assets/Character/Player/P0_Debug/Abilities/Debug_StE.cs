using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_StE : PA_StatusEffect
{
    [SerializeField] Action.ActionStatus actionStatus;
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn) Enqueue_Self(actionStatus);
    }
}
