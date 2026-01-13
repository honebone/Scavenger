using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_RuneCrafter : PA_Personality
{
    [SerializeField] int maxHPPerRepair;
    [SerializeField] int ATKINTPerRepair;
    List<PassiveAbility> repaired = new List<PassiveAbility>();

    public override void OnRuneActivate(PassiveAbility rune)
    {
        if (rune.GetRuneCharge() == 0 && !repaired.Contains(rune))
        {
            Log($"{rune.GetPAName()}‚ğC—");
            repaired.Add(rune);
            rune.ChargeRune(rune.GetRuneInitialCharge());

            character.AddMaxHP(0, maxHPPerRepair, true);
            character.AddATKINT(0, ATKINTPerRepair);
        }
    }

    public override void OnBattleEnd()
    {
        character.AddMaxHP(0, -maxHPPerRepair * repaired.Count, true);
        character.AddATKINT(0, -ATKINTPerRepair * repaired.Count);
        repaired = new List<PassiveAbility>();
    }

    public override string GetCurrentStateInfo()
    {
        int count = repaired.Count;
        if (count > 0)
        {
            string s = "";
            repaired.ForEach(r => { s += $"{Extentions.NL(s, lineStr: ",")}{r.GetPAName()}"; });
            return $"C—‚µ‚½ƒ‹[ƒ“F{repaired.Count}ŒÂ({s})\n{"maxHP".ToSpr_withName()}+{maxHPPerRepair * count}“\n{"ATK".ToSpr_withName()}+{ATKINTPerRepair * count}“\n{"INT".ToSpr_withName()}+{ATKINTPerRepair * count}“";
        }
        else return "";
    }
}
