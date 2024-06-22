using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Warrior_Calm : ActionMod
{
    [SerializeField] GameObject anger;
    [SerializeField] int valuePerStack;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        int value = statusRef.actionOwner.GetStEStack(anger) * valuePerStack;
        //FindObjectOfType<InfoText>().AddDebugText(value.ToString());

        if (value > 0)
        {
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                FindObjectOfType<InfoText>().AddDebugText(value.ToString());
                actionModStatus.healRegain = value;
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }

        return actionsStatus;
    }
}
