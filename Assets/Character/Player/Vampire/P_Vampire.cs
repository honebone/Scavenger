using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Vampire : PA_Personality
{
    [SerializeField] int HPPercentTH;
    [SerializeField] int EVDPerActivate;
    [SerializeField] int maxActivate;

    int count;
    int activeCount;
    public override void OnDecreasedHP(int value)
    {
        if (value == 0) return;
        if (activeCount == maxActivate) return;
        count += value;
        LogCount(count, value);
        int TH = character.CharaStatus().maxHP.Mul(HPPercentTH);
        if (TH == 0) return;
        while(count >= TH)
        {
            count -= TH;
            activeCount++;
            character.AddEVD(EVDPerActivate);
            Log($"{"EVD".ToSpr_withLink()}+{EVDPerActivate}");

            if (activeCount == maxActivate) break;
        }
    }

    public override void OnBattleEnd()
    {
        if(activeCount > 0)
        {
            character.AddEVD(-EVDPerActivate * activeCount);
            activeCount = 0;
        }
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}/{character.CharaStatus().maxHP.Mul(HPPercentTH)}\n増加した{"EVD".ToSpr_withLink()}：{EVDPerActivate * activeCount}/{EVDPerActivate * maxActivate}";
    }
}
