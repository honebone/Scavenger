using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ReactiveArmor : PA_StatusEffect
{
    [SerializeField] Action.ActionStatus actionStatus;

    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (onDamageParams.totalDMG > 0&& !character.CharaStatus().dead)
        {
            Action.ActionStatus action = actionStatus;
            action.shieldPercent_min = StEStatus.value;
            action.shieldPercent_max = StEStatus.value;
            Enqueue_Self(action);

            AddStack(-1);
        }
    }

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn) AddStack(-1);
    }
}
