using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_Missile : Eq_Magic
{
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;

    public override void OnCast(Eq_Magic cast)
    {
        if (cast != this) Cast();
    }

    public override void Cast()
    {
        if (Enqueue_SearchTarget(actionStatus, condition, 1)) character.OnCast(this);
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }
}
