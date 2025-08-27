using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Focus : ActionMod
{
    [SerializeField] bool onlyAttack;
    [SerializeField] bool onlyAbility;
    [SerializeField] bool onlyPassive;
    public bool notFocus;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (!statusRef.DoesAttack() && onlyAttack) return actionsStatus;
        if (!statusRef.abilityEffect && onlyAbility) return actionsStatus;
        if (statusRef.abilityEffect && onlyPassive) return actionsStatus;

        //actionModStatus.consumeFocus = true;
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if (statusRef.actionTargets[i].CharaStatus().focused > 0 != notFocus)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }

        return actionsStatus;
    }
}
