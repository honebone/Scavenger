using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqR_Inguz : Eq_Rune
{
    public Action.ActionStatus attack;
    public Action.ActionStatus heal;
    public CharactersManager.SearchCharaCondition condition;

    public override void OnBattleStart()
    {
        RuneInitialCharge();
    }
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        RuneActivate();
    }

    public override void RuneActivation()
    {
        Enqueue_Self(heal);
    }

    public override void OnHealed(Character healer, Action.OnHealParams onHealParams)
    {
        Enqueue_SearchTarget(attack, condition,1);
    }

    public override string GetPAInfo_Base()
    {
        string s = attack.GetInfo() + "\n" + heal.GetInfo();
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return $"チャージ：{runeCharge}";
    }
}
