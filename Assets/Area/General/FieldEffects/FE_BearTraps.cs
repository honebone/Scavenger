using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FE_BearTraps : FieldEffect
{
    [SerializeField]
    Action.ActionStatus trap;

    List<int> pos = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

    public override string GetFEInfo()
    {
        return trap.GetInfo(false, new Character.CharacterStatus());
    }
    public override void OnBattleStart()
    {
        Action.ActionStatus action = trap;

        action.actionOwner = null;
        action.actionTargetsInt = pos.Sample(2);
        FindObjectOfType<ActionQueueManager>().Enqueue(action);
    }
}
