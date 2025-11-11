using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_ReapersFinger : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus action;

    public override string GetPAInfo_Base()
    {
        string s = action.GetInfo();
        return s;
    }
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn && (character.CharaStatus().CRITC * 2f).Dice())
        {
            character.Enqueue(action, true, new List<Character> { character });
        }
    }
}
