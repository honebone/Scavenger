using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;

public class Eq_FoldingFan : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += actionStatus.GetInfo();
        return s;
    }

    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        if (onAttackParams.evaded)
        {
            Action.ActionStatus action = actionStatus;
            List<Character> targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition));
            actionStatus.actionOwner = character;
            character.Enqueue(action, true, targets);
        }
    }
}
