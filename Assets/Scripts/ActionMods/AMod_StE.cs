using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_StE : ActionMod
{
    [SerializeField]
    GameObject checkStE;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if (statusRef.actionTargets[i].CheckHasStE(checkStE))
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        return actionsStatus;
    }
}
