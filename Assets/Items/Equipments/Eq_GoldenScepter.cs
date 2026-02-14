using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_GoldenScepter : PA_Equipment
{
    [SerializeField] float exDMGPerCoin;
    float value;
    public override void OnBattleStart()
    {
        value=exDMGPerCoin*Inventory.inst.GetCoin();
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
