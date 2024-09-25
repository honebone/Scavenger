using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_HPPercentDiff : ActionMod
{
    [SerializeField] bool ownerIsGreater;
    [SerializeField] bool allowEqual;
    [SerializeField] bool onlyAttack;

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        float selfHP = statusRef.actionOwner.GetCharacterStatus().GetHPPercent();

        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            float targetHP = statusRef.actionTargets[i].GetCharacterStatus().GetHPPercent();
            if ((statusRef.DoesAttack() || !onlyAttack) && (selfHP > targetHP) == ownerIsGreater && (selfHP == targetHP) == allowEqual)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }

        return actionsStatus;
    }

}
