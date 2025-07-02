using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_TargetAmount : ActionMod
{
    public bool onlyAttack;
    [Header("TH‚ð’´‚¦‚½1l‚É‚Â‚«")] public int TH = 0;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (statusRef.actionTargets.Count > TH && (statusRef.DoesAttack() || !onlyAttack))
        {
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                for (int j = TH; j < statusRef.actionTargets.Count; j++)
                {
                    actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
                }
            }
        }

        return actionsStatus;
    }
}
