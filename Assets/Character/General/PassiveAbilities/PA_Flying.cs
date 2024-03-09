using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Flying : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    public override void OnBattleStart()
    {
        Action.ActionStatus action = actionStatus;
        actionStatus.actionOwner = character;

        character.Enqueue(action, true, new List<Character> { character });
    }

    public override string GetPAInfo()
    {
        return actionStatus.GetInfo(true, character.GetCharacterStatus());
    }
}
