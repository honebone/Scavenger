using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Summon : ActionMod
{
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        Debug.Log("ok");

        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            Debug.Log("ok1");

            if (statusRef.summon)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        return actionsStatus;
    }
}
