using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_BloodyBlade : Eq_Magic
{
    [SerializeField] int HPTHPercent;
    [SerializeField] int HPMod;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    int count;

    int countGoal => character.CharaStatus().maxHP.Mul(HPTHPercent);

    public override void OnDecreasedHP(int value)
    {
        count += value;
        LogCount(count, value);
        while (count >= countGoal)
        {
            Cast();
            count -= countGoal;
        }
    }

    public override void Cast()
    {
        Action.ActionStatus action = actionStatus;
        int exDMG = character.CharaStatus().maxHP.Mul(HPMod);
        action.INTMod_max += exDMG;
        action.INTMod_min += exDMG;

        if (Enqueue_SearchTarget(action, condition, 1))
        {
            character.OnCast(this);
        }
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}/{countGoal}";
    }
}
