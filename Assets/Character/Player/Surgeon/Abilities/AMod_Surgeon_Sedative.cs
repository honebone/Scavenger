using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Surgeon_Sedative : ActionMod
{
    [SerializeField]
     ActionModStatus actionModStatus_player;

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        Character.CharacterStatus ownerStatus = statusRef.actionOwner.CharaStatus();
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if(statusRef.actionTargets[i].CharaStatus().position.IsPlayerPos()) actionsStatus[i] = actionsStatus[i].Modify(actionModStatus_player);
            else actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
        }

        return actionsStatus;
    }
}
