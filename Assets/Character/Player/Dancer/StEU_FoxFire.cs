using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;

public class StEU_FoxFire : PA_StatusEffect
{
    [SerializeField] int countReq;
    [SerializeField] int maxEVD;
    [SerializeField] int burnPerEVD;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    int count;

    public override void OnRoundEnd()
    {
        ActionStatus action = actionStatus;
        List<StatusEffectParams> list= new List<StatusEffectParams>(action.applySteParams);
        StatusEffectParams burn = list[0];
        int EVD = Mathf.Min(maxEVD, character.CharaStatus().EVD).ToInt();
        burn.value += EVD * burnPerEVD;

        list[0] = burn;
        action.applySteParams = new List<StatusEffectParams>(list);

        Enqueue_SearchTarget(action, condition);
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
