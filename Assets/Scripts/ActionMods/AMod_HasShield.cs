using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_HasShield : ActionMod
{
    [SerializeField,Header("シールドがあるときに発動するか、ないときか")] bool hasShield;
    [SerializeField] bool onlyAttack;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if ((statusRef.DoesAttack() || !onlyAttack) && (statusRef.actionTargets[i].GetCharacterStatus().shield > 0 == hasShield))
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        return actionsStatus;
    }
}
