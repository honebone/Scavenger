using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_GoblinLeader_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;
    [SerializeField]
    Action.ActionStatus summon;
    [SerializeField] int summonInterval;
    List<int> list = new List<int>() { 9, 10, 11, 12, 13, 14, 15, 16, 17 };
    List<int> empty = new List<int>();

    int roundCount;
    public override void OnDie(Character killer)
    {
        Action.ActionStatus action = actionStatus;
        actionStatus.actionOwner = character;
        Enqueue(action, true, charactersManager.SearchCharaWithCondition(condition));
    }

    public override void OnRoundEnd()
    {
        roundCount++;
        if (roundCount % summonInterval == 0)
        {
            Action.ActionStatus action = summon;
            actionStatus.actionOwner = character;
            empty = charactersManager.GetEmptyPos(list);
            action.actionTargetsInt = new List<int>(empty.Sample(3));
            Enqueue(action, false, new List<Character>());
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo(true, character.GetCharacterStatus()) + "\n";
        s+= summon.GetInfo(true, character.GetCharacterStatus());
        return s;
    }
}
