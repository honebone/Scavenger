using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_AttackType : ActionMod
{
    [SerializeField,Header("0:melee 1:ranged 2:magic")]
    List<int> checkType;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if (statusRef.ATKMod_max > 0 && checkType.Contains(statusRef.attackType))
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        return actionsStatus;
    }
}
