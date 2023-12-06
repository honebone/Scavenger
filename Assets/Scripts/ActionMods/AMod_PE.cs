using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_PE : ActionMod
{
    [SerializeField]
    GameObject checkPE;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if (statusRef.actionTargets[i].CheckHasPE(checkPE))
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        return actionsStatus;
    }
}
