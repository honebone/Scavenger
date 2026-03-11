using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionMod;

public class AMod_Attack : ActionMod
{
    [SerializeField] bool onlyActive;
    [SerializeField] bool onlyPassive;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if(!statusRef.DoesAttack()) return actionsStatus;
        if (onlyActive&&!statusRef.abilityEffect) return actionsStatus;
        if (onlyPassive&&statusRef.abilityEffect) return actionsStatus;

        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
        }

        return actionsStatus;
    }
}
