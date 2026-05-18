using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StEU_FoxFire : PA_StatusEffect
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnRoundEnd()
    {
        Enqueue_SearchTarget(actionStatus, condition);
        AddStack(-1);
    }

    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        if (onAttackParams.evaded) AddStack(1);
    }
    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo();
    }
}
