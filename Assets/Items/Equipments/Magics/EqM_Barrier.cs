using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_Barrier : Eq_Magic
{
    public Action.ActionStatus actionStatus;

    bool act;

    public override void OnBattleStart()
    {
        act = false;
    }

    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        act = true;
    }
    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (!act && tep.myTurn)
        {
            Cast();
        }
        act = false;
    }

    public override void Cast()
    {
        Enqueue_Self(actionStatus);
        character.OnCast(this);
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
}
