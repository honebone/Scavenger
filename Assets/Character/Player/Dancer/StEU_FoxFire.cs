using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StEU_FoxFire : PA_StatusEffect
{
    [SerializeField] int countReq;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    int count;

    public override void OnRoundEnd()
    {
        Enqueue_SearchTarget(actionStatus, condition);
        AddStack(-1);
    }

    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        if (onAttackParams.evaded)
        {
            count++;
            LogCount(count);
            if (count == countReq)
            {
                AddStack(1);
                count = 0;
            }
        }
    }
    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo();
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}/{countReq}";
    }
}
