using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FE_NarrowRoom : FieldEffect
{
    [SerializeField]
    Action.ActionStatus narrow;

    public override string GetFEInfo()
    {
        return narrow.GetInfo(false, new Character.CharacterStatus());
    }
    public override void OnBattleStart()
    {
        Action.ActionStatus action = narrow;

        action.actionOwner = null;
        action.actionTargetsInt = new List<int> { 0,2,3,5,6,8,9,11,12,14,15,17};
        FindObjectOfType<ActionQueueManager>().Enqueue(action);
    }
}
