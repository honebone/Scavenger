using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Per_Toxic : PA_Personality
{
    [SerializeField]
    int chance;
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn)
        {
            if (chance.Dice())
            {
                Action.ActionStatus action = actionStatus;
                List<Character> targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition));
                targets.Remove(character);
                if (targets.Count > 0)
                {
                    actionStatus.actionOwner = character;
                    Enqueue(action, true, new List<Character>() { targets.Choice() });
                }
            }
        }
    }
}
