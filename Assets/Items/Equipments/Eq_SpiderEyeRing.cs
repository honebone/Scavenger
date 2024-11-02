using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_SpiderEyeRing : PA_Equipment
{
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    [SerializeField] float DMGPerTarget;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.actionTargets.Count > 1&&statusRef.DoesAttack())
        {
            ActionMod.ActionModStatus mod = actionModStatus;
            float exDMG = DMGPerTarget * statusRef.actionTargets.Count;
            mod.exDMG_mul = exDMG;
            Debug.Log(mod.exDMG_mul);
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(mod);
            }
        }

        return actionsStatus;
    }


}
