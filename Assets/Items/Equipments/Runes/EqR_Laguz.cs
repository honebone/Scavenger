using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqR_Laguz : Eq_Rune
{
    public Action.ActionStatus actionStatus;

    public override void OnBattleStart()
    {
        RuneInitialCharge();
    }

    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        if (onAttackParams.evaded)
        {
            RuneActivate();
        }
    }

    public override void RuneActivation()
    {
        Enqueue_Self(actionStatus);
    }

    public override void OnBattleEnd()
    {
        ResetRuneCharge();
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
