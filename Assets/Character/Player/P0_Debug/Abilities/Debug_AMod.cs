using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_AMod : ActionMod
{
    [SerializeField] int amount;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            for (int j = 0; j < amount; j++) {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }

        return actionsStatus;
    }
}
