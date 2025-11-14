using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_Spear : Eq_Magic
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        Cast();
    }

    public override void Cast()
    {
        if (Enqueue_SearchTarget(actionStatus, condition)) character.OnCast(this);
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
}
