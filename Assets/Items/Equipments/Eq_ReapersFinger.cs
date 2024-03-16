using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_ReapersFinger : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus action;

    public override string GetPAInfo()
    {
        string s = equipmentStatus.GetInfo() + "\n\n";
        s += action.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn && (character.GetCharacterStatus().CRITC * 2f).Probability())
        {
            character.Enqueue(action, true, new List<Character> { character });
        }
    }
}
