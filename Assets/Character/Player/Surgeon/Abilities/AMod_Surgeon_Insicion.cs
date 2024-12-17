using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Surgeon_Insicion : ActionMod
{
    [SerializeField]
    float ratio;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        Character.CharacterStatus ownerStatus = statusRef.actionOwner.CharaStatus();
        actionModStatus.CRITCMod = Mathf.RoundToInt(ownerStatus.INT * ratio);
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
        }

        return actionsStatus;
    }
}
