using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Shovel : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn)
        {
            List<Character> target = charactersManager.SearchCharaWithCondition(condition);
            if (target.Count > 0)
            {
                Enqueue(actionStatus, true, target, 1);
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
}
