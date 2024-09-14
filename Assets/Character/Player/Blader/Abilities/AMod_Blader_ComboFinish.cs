using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Blader_ComboFinish : ActionMod
{
    [SerializeField] GameObject combo;
    [SerializeField] int ATKModPerStack;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        int combos = statusRef.actionOwner.GetStEStack_Sum(combo);
        actionModStatus.ATKMod = ATKModPerStack * combos;

        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
        }

        return actionsStatus;
    }
}
