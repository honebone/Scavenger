using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_OnRoundStart : Eq_Magic
{
    public bool targetSelf;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public int targetCount;
    public override void OnRoundStart()
    {
        Cast();
    }

    public override void Cast()
    {
        if (targetSelf)
        {
            Enqueue_Self(actionStatus);
            character.OnCast(this);
        }
        else
        {
            if (Enqueue_SearchTarget(actionStatus, condition, targetCount))
            {
                character.OnCast(this);
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }
}
