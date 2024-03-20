using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_GoblinLeader_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;
    public override void OnDie(Character killer)
    {
        Action.ActionStatus action = actionStatus;
        actionStatus.actionOwner = character;
        character.Enqueue(action, true, charactersManager.SearchCharaWithCondition(condition));
    }


    public override string GetPAInfo()
    {
        return actionStatus.GetInfo(true, character.GetCharacterStatus());
    }
}
