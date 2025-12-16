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
        RuneInitialCharge();
    }

    public override void OnDamage(List<OnDamageParams> onDamageParamsList)
    {
        onDamageParamsList.ForEach(x =>
        {
            if (x.CRIT && runeCharge > 0)
            {
                RuneActivate();
                return;
            }
        });
    }

    public override void RuneActivation()
    {
        count++;
        character.AddCRITD(CRITD);
        Log($"{"CRIT".ToSpr_withName()}ダメージ+{CRITD}％ ({count * CRITD}％)");
    }

    public override void OnBattleEnd()
    {
        character.AddCRITD(-count * CRITD);
        count = 0;
        ResetRuneCharge();
    }

    public override string GetCurrentStateInfo()
    {
        return $"チャージ：{runeCharge}\n{"CRIT".ToSpr_withName()}ダメージ+{count * CRITD}％";
    }
}
