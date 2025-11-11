using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StEU_PestQueen_Egg : PA_StatusEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    List<int> list = new List<int>() { 9, 10, 11, 12, 13, 14, 15, 16, 17 };
    List<int> empty = new List<int>();

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn)
        {
            Action.ActionStatus action = actionStatus;
            empty = charactersManager.GetEmptyPos(list);
            if(empty.Count > 0)
            {
                action.actionTargetsInt = empty;
                character.Enqueue(action, false, new List<Character>(), 1);
                AddStack(-1);
            }
        }
    }

    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }
}
