using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_Complexed : PA_Personality
{
    [SerializeField] float exDMGPerPer;
    float value;
    public override void OnBattleStart()
    {
        value = exDMGPerPer * character.GetPers_Rand().Count;
        character.AddExDMG_Mul(value);
        Log($"与ダメージ+{value}％");
    }
    public override void OnBattleEnd()
    {
        character.AddExDMG_Mul(-value);
        value = 0;
    }
    public override string GetCurrentStateInfo()
    {
        return $"与ダメージ+{value}％";
    }
}
