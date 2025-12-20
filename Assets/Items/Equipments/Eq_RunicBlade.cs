using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eq_RunicBlade : PA_Equipment
{
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn && character.GetRunes().Count > 0)
        {
            character.GetRunes().Choice().ChargeRune(1);
        }
    }

    public override void OnRuneActivate(PassiveAbility rune)
    {
        Enqueue_SearchTarget(actionStatus, condition, 1);
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
}
