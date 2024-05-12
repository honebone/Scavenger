using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Focus : ActionMod
{
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if (statusRef.actionTargets[i].GetCharacterStatus().focused > 0)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        return actionsStatus;
    }
}
