using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_Arrow : Eq_Magic
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition_focus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn&& charactersManager.SearchCharaWithCondition(condition_focus).Count>0)Cast();
    }

    public override void Cast()
    {
        List<Character> targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition_focus));
        if (targets.Count == 0) targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition));

        Enqueue(actionStatus, true, targets,1);
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo();
        return s;
    }
}
