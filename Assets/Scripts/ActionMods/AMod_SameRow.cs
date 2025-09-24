using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionMod;

public class AMod_SameRow : ActionMod
{
    public bool onlyAttack;
    public bool onlyAlly;
    [SerializeField, Header("オーナーから見て敵！")] bool onlyOpponent;
    public bool onlyAbility;
    public bool onlyPassive;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (!statusRef.DoesAttack() && onlyAttack) return actionsStatus;
        Character owner = statusRef.actionOwner;
        if (!statusRef.abilityEffect && onlyAbility) return actionsStatus;
        if (statusRef.abilityEffect && onlyPassive) return actionsStatus;

        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            Character target = statusRef.actionTargets[i];
            if (owner.PlayerPos() == target.PlayerPos()) { if (onlyOpponent) continue; }
            else if (onlyAlly) continue;
            if (target.CharaStatus().position.GetRow() == owner.CharaStatus().position.GetRow()) actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
        }

        return actionsStatus;
    }
}
