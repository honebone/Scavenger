using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_SpreadShot : Eq_Magic
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if ( tep.myTurn&&!tep.actThisTurn)
        {
            Cast();
            Cast();
            Cast();
        }
    }

    public override void Cast()
    {
        if (Enqueue_SearchTarget(actionStatus, condition, 1))
        {
            character.OnCast(this);
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
}
