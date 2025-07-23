using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_Fireball : Eq_Magic
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    int count;

    public override void OnRoundStart()
    {
        count++;
        if (count % 2 == 1) { Cast(); }
    }

    public override void Cast()
    {
        if(Enqueue_SearchTarget(actionStatus, condition, 1)) character.OnCast(this);
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }
}
