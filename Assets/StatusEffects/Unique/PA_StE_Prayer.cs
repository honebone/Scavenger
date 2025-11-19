using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Prayer : PA_StatusEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    [SerializeField]
    int stackThreshold;
    public override void OnAddStack(int add)
    {
        if (StEStatus.stack >= stackThreshold)
        {
            Enqueue_Self(actionStatus);
        }
    }
    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo();
    }
}
