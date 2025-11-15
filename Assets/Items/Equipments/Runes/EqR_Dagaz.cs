using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;

public class EqR_Dagaz : Eq_Rune
{
    //public int maxCountConsumePercent;
    public int countPercentOnActivate;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;

    int count;

    public override void OnBattleStart()
    {
        RuneInitialCharge();
    }
    public override void OnRoundStart()
    {
        RuneActivate();
    }

    public override void RuneActivation()
    {
        int add = (CharaStatus().maxHP * countPercentOnActivate / 100f).ToInt();
        count += add;
        Log($"カウント+{add} ({count})");
    }

    public override void OnHealed(Character healer, Action.OnHealParams onHealParams)
    {
        count += onHealParams.healValue;
        Log($"カウント+{onHealParams.healValue} ({count})");
    }
    public override void OnDecreasedHP(int value)
    {
        count += value;
        Log($"カウント+{value} ({count})");
    }

    public override void OnRoundEnd()
    {
        Action.ActionStatus action= actionStatus;
        action.INTDMG_divide_int = count;
        count = 0;
        Enqueue_SearchTarget(action, condition);
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return $"チャージ：{runeCharge}\nカウント：{count}";
    }
}
