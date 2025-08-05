using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Per_OnMyTurnStart : PA_Personality
{
    public bool targetSelf;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public int targetCount;

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn)
        {
            if (targetSelf) { Enqueue_Self(actionStatus); }
            else { Enqueue_SearchTarget(actionStatus, condition, targetCount); }
        }
    }
    

    public override string GetPAInfo_Base()
    {
        string s="";
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
