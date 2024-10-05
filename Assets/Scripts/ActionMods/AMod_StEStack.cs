using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_StEStack : ActionMod
{
    [SerializeField] GameObject checkStE;
    [SerializeField,Header("falseなら対象をチェック")] bool checkOwner;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        int stack = 0;
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            stack = (checkOwner) ? statusRef.actionOwner.GetStEStack_Sum(checkStE) : statusRef.actionTargets[i].GetStEStack_Sum(checkStE);
            for (int j = 0; j < stack; j++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        return actionsStatus;
    }
}
