using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_SingleTarget : ActionMod
{
    public bool onlyAttack;
    [SerializeField] bool onlyActive;

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (!statusRef.abilityEffect && onlyActive) return actionsStatus;

        if (statusRef.actionTargets.Count == 1 && (statusRef.DoesAttack() || !onlyAttack))
        {
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }

        return actionsStatus;
    }
}
