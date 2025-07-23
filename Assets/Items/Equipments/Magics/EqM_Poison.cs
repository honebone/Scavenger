using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_Poison : Eq_Magic
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;


    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn)
        {
            Cast();
        }
    }

    public override void Cast()
    {
        List<Character> targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition));

        if (Enqueue(actionStatus, true, targets, 2))
        {
            character.OnCast(this);
        }

    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo();
        return s;
    }

}
