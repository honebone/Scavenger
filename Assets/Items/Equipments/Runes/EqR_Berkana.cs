using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Action;

public class EqR_Berkana : Eq_Rune
{
    [SerializeField] int maxHP;
    int count;
    public override void OnBattleStart()
    {
        RuneInitialCharge();
    }
    public override void OnApplyedStE(OnApplyStEParams onApplyStEParams)
    {
        onApplyStEParams.appliedParams.ForEach(x =>
        {
            if (x.GetStEType() == PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff) { RuneActivate(); }
        });
    }

    public override void RuneActivation()
    {
        count++;
        character.AddMaxHP(0,maxHP,true);
        Log($"{"maxHP".ToSpr_withName()}+{maxHP}％ ({count * maxHP}％)");
    }

    public override void OnBattleEnd()
    {
        character.AddMaxHP(0, -count * maxHP, true);
        count = 0;
        ResetRuneCharge();
    }

    public override string GetCurrentStateInfo()
    {
        return $"チャージ：{runeCharge}\n{"maxHP".ToSpr_withName()}+{count * maxHP}％";
    }
}
