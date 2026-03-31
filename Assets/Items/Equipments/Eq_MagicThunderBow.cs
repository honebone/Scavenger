using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_MagicThunderBow : PA_Equipment
{
    [SerializeField] int INTModPerCount;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    int count = 0;

    public override void OnCast(PassiveAbility cast)
    {
        count++;
        Log($"カウント+1 ({count})");
    }

    public override void OnRoundEnd()
    {
        Action.ActionStatus action = actionStatus;
        action.INTMod_max += count * INTModPerCount;
        action.INTMod_min += count * INTModPerCount;

        Enqueue(action,true,charactersManager.SearchChara_Strongest(condition,false),1);
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}\n{"INT".ToSpr_withName()}補正+{count * INTModPerCount}％";
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }
}
