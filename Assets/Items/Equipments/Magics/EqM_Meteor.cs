using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_Meteor : Eq_Magic
{
    public int ACCTH;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn && character.CharaStatus().ACC >= ACCTH)
        {
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
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo();
        return s;
    }
}
