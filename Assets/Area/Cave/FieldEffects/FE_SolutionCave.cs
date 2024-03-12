using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FE_SolutionCave : FieldEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    List<int> list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
    List<int> empty = new List<int>();

    public override string GetFEInfo()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }
    public override void OnBattleStart()
    {
        Action.ActionStatus action = actionStatus;

        action.actionOwner = null;
        empty = new List<int>(charactersManager.GetEmptyPos(list));
        action.actionTargetsInt = new List<int>(empty.Sample(3));
        FindObjectOfType<ActionQueueManager>().Enqueue(action);
    }
}
