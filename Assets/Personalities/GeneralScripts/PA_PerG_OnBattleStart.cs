using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PA_Equipment;

public class PA_PerG_OnBattleStart : PA_Personality
{
    [Header("‚±‚ê‚Í‹Œ®(Per_OnBattleStart‚ªVŒ^)")]
    public bool targetSelf;
    public bool excludeSelf;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public int targetCount;

    public override void OnBattleStart()
    {
        Activate();
    }

    void Activate()
    {
        if (targetSelf)
        {
            Enqueue_Self(actionStatus);
        }
        else
        {
            List<Character> list = charactersManager.SearchCharaWithCondition(condition);
            if(excludeSelf)list.Remove(character);
            Enqueue(actionStatus, true, list, targetCount);
        }
    }

   

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
