using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Heal : ActionMod
{
    [SerializeField] bool onlyToPlayer;
    [SerializeField] bool onlyActive;
    [SerializeField] bool onlyPassive;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (!statusRef.DoesHeal()) return actionsStatus;
        if (onlyActive && !statusRef.abilityEffect) return actionsStatus;
        if (onlyPassive && statusRef.abilityEffect) return actionsStatus;

        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if (onlyToPlayer && !statusRef.actionTargets[i].IsPlayer()) continue;
            actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
        }

        return actionsStatus;
    }
}
