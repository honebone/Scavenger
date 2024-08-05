using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Active : ActionMod
{
    [SerializeField, Header("true:アビリティ false:誘発")] bool checkActive;
    [SerializeField] bool onlyAttack;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if ((statusRef.ATKMod_max > 0 || !onlyAttack) && (statusRef.abilityEffect == checkActive))
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        return actionsStatus;
    }
}
