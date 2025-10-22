using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Counter : PA_StatusEffect
{
    [SerializeField] bool INTCounter;
    [SerializeField]
    Action.ActionStatus actionStatus;

    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        if (!character.CharaStatus().dead && onAttackParams.actionParams.actionStatus.abilityEffect)
        {
            Action.ActionStatus action = actionStatus;
            if (!INTCounter)
            {
                action.ATKMod_min = StEStatus.value;
                action.ATKMod_max = StEStatus.value;
            }
            else
            {
                action.INTMod_min = StEStatus.value;
                action.INTMod_max = StEStatus.value;
            }

            actionStatus.actionOwner = character;
            Enqueue(action, true, new List<Character>() { onAttackParams.actionParams.owner });

            AddStack(-1);
        }
    }
    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn && applyFlag) { AddStack(-1); }
    }
}
