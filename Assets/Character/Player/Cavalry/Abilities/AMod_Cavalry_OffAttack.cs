using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Cavalry_OffAttack : ActionMod
{
    public float exDMGPerHP;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (statusRef.actionOwner.CharaStatus().GetHPPercent() >= 50)
        {
            ActionModStatus mod = actionModStatus;
            float ratio = statusRef.actionOwner.CharaStatus().GetHPPercent() - 50;
            float DMGMod = exDMGPerHP * ratio;
            mod.exDMG_mul = DMGMod;
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(mod);
            }
        }
        return actionsStatus;
    }
}
