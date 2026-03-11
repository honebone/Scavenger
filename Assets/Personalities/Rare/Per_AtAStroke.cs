using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_AtAStroke : PA_Personality
{
    [SerializeField] int countReq;
    [SerializeField] int ATK;
    int count;
    int ATKCount;

    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        if (count < countReq)
        {
            count++;
            Log($"カウント+1 ({count})");
            if (count == countReq)
            {
                ATKCount++;
                character.AddATK(0, ATK);
                Log($"{"ATK".ToSpr_withLink()}+{ATK}％ (+{ATK * ATKCount}％)");
            }
        }
    }

    public override void OnRoundStart()
    {
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}/{countReq}\n{"ATK".ToSpr_withLink()}+{ATK * ATKCount}％";
    }

    public override void OnBattleEnd()
    {
        character.AddATK(0, -ATK * ATKCount);
        count = 0;
        ATKCount = 0;
    }
}
