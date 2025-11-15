using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PA_Equipment;

public class EqR_OnRoundStart : Eq_Rune
{
    public bool targetSelf;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public int targetCount;

    public override void OnBattleStart()
    {
        RuneInitialCharge();
    }
    public override void OnRoundStart()
    {
        if (runeCharge > 0) { Activate(); }
    }

    void Activate()
    {
        if (targetSelf)
        {
            Enqueue_Self(actionStatus);
            RuneActivate();
        }
        else
        {
            if (Enqueue_SearchTarget(actionStatus, condition, targetCount))
            {
                RuneActivate();
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return $"チャージ：{runeCharge}";
    }
}
