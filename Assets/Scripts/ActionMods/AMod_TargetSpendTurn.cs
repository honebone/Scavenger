using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_TargetSpendTurn : ActionMod
{
    public bool onlyAttack;
    public bool notSpendTurn;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (!statusRef.DoesAttack() && onlyAttack) return actionsStatus;

        //actionModStatus.consumeFocus = true;
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if ((statusRef.actionTargets[i].CharaStatus().spendTurn == 0) == notSpendTurn)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }

        return actionsStatus;
    }
}
