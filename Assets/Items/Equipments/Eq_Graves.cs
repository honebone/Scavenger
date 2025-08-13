using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Graves : PA_Equipment
{
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public CharactersManager.SearchCharaCondition diedCond;
    int count;
    public override void OnSomeoneDied(Character died)
    {
        if (charactersManager.ExamineCharacter(died, diedCond))
        {
            count++;
            Log($"カウント+1 ({count})");
        }
    }

    public override void OnRoundEnd()
    {
        if (count > 0)
        {
            Enqueue_SearchTarget(actionStatus, condition, count);
            count = 0;
        }
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo();
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}";
    }
}
