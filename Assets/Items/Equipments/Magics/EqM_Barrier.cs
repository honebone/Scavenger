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
    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (!act && myTurn)
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
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
