using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Per_Fashionista : PA_Personality
{
    [SerializeField] int value_epic;
    [SerializeField] int value_legendary;
    int value;
    public override void OnBattleStart()
    {
        character.CharaStatus().equipments.ForEach(eq =>
        {
            if (eq.data.rarity == ItemData.Rarity.epic) value += value_epic;
            else if (eq.data.rarity == ItemData.Rarity.legendary) value += value_legendary;
        });
        character.AddATKINT(0,value);
        Log($"{"ATK".ToSpr_withName()}{"INT".ToSpr_withName()}+{value}Åì");
    }
    public override void OnBattleEnd()
    {
        character.AddATKINT(0, -value);
        value = 0;
    }
    public override string GetCurrentStateInfo()
    {
        return $"{"ATK".ToSpr_withName()}{"INT".ToSpr_withName()}+{value}Åì";
    }
}
