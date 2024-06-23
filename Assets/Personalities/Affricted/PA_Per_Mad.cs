using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Per_Mad : PA_Personality
{
    [SerializeField]
    int chance;
    [SerializeField]
    Action.ActionStatus SANDMG;
    [SerializeField]
    Action.ActionStatus mark;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;

    public override string GetPAInfo_Base()
    {
        string s= SANDMG.GetInfo(false, new Character.CharacterStatus());
        s+= mark.GetInfo(false, new Character.CharacterStatus());
        return s;
    }

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn)
        {
            if (chance.Dice())
            {
                Action.ActionStatus action = SANDMG;
                List<Character> targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition));
                targets.Remove(character);
                if (targets.Count > 0)
                {
                    SANDMG.actionOwner = character;
                    Enqueue(action, true, targets);
                }
                Action.ActionStatus action2 = mark;
                mark.actionOwner = character;
                Enqueue(action2, true, new List<Character>() { character });
            }
        }
    }
}
