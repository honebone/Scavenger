using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Action;

public class EqR_Sowelu : Eq_Rune
{
    public int CRITD;
    int count;
    public override void OnBattleStart()
    {
        ChargeRune(rune_initialCharge);
    }

    public override void OnDamage(List<OnDamageParams> onDamageParamsList)
    {
        onDamageParamsList.ForEach(x =>
        {
            if (x.CRIT && runeCharge > 0)
            {
                Activate();
                return;
            }
        });
    }

    void Activate()
    {
        RuneActivate();
        count++;
        character.AddCRITD(CRITD);
        Log($"{"CRIT".ToSpr_withName()}ダメージ+{CRITD}％ ({count * CRITD}％)");
    }

    public override void OnBattleEnd()
    {
        character.AddCRITD(-count * CRITD);
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"{"CRIT".ToSpr_withName()}ダメージ+{count * CRITD}％";
    }
}
