using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Per_OnDie : PA_Personality
{
    public bool hasSelfCondition;
    public CharactersManager.SearchCharaCondition selfCondition;
    public bool targetSelf;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public int targetCount;

    public override void OnDie(Character killer)
    {
        if (!hasSelfCondition || charactersManager.ExamineCharacter(character, selfCondition))
        {
            if (targetSelf) { Enqueue_Self(actionStatus); }
            else { Enqueue_SearchTarget(actionStatus, condition, targetCount); }
        }
    }


    public override string GetPAInfo_Base()
    {
        string s = "";
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
