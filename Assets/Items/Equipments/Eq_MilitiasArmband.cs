using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_MilitiasArmband : PA_Equipment
{
    [SerializeField] int valuePerEq;
    int currentValue;

    public override void OnBattleStart()
    {
        int uncommons = 0;
        foreach(PA_Equipment equipment in character.GetEquipments())
        {
            if (equipment.GetEquipmentStatus().itemData.rarity == ItemData.Rarity.uncommon) { uncommons++; }
        }
        currentValue = uncommons * valuePerEq;
        character.AddATKINT(0, currentValue);
    }
    public override void OnBattleEnd()
    {
        character.AddATKINT(0, -currentValue);
        currentValue = 0;
    }
    public override string GetCurrentStateInfo()
    {
        return $"åªç›ÅF+{currentValue}Åì";
    }
}
