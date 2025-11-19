using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;

public class EqR_Blank : Eq_Rune
{
    public int chargeMul;
    public override void OnBattleStart()
    {
        RuneInitialCharge();
        character.GetRunes().ForEach(x =>
        {
            if (x != this) x.ChargeRune(x.rune_initialCharge * chargeMul);
        });
    }

    public override void OnRoundStart()
    {
        RuneActivate();
    }

    public override void RuneActivation()
    {
        character.GetRunes().ForEach(x =>
        {
            if (x != this) x.RuneActivate();
        });
    }
    public override void OnBattleEnd()
    {
        ResetRuneCharge();
    }

    public override string GetCurrentStateInfo()
    {
        return $"チャージ：{runeCharge}";
    }
}
