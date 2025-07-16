using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Summon : ActionMod
{
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (statusRef.summon)
        {
            for (int i = 0; i < statusRef.actionTargetsInt.Count; i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        
        return actionsStatus;
    }
}
