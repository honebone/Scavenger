using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_FoldingFan : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += equipmentStatus.GetInfo();
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }

    public override void OnAttacked(Character attacker, bool evaded, bool missed)
    {
        if (evaded)
        {
            Action.ActionStatus action = actionStatus;
            List<Character> targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition));
            actionStatus.actionOwner = character;
            character.Enqueue(action, true, targets);
        }
    }
}
