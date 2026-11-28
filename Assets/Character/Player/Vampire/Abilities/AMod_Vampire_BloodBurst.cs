using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Vampire_BloodBurst : ActionMod
{
    [SerializeField] int healMul;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        ActionModStatus mod = actionModStatus;
        mod.healValue = statusRef.actionOwner.CharaStatus().shield.Mul(healMul);

        for(int i = 0; i < actionsStatus.Length; i++)
        {
            actionsStatus[i] = actionsStatus[i].Modify(mod);
        }

        return actionsStatus;
    }
}
