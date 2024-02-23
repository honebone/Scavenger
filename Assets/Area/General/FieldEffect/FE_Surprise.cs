using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FE_Surprise : FieldEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;

    public override string GetFEInfo()
    {
        return "error";
    }
    public override void OnBattleStart()
    {
        Action.ActionStatus action_player = actionStatus;

        action_player.actionOwner = null;
        action_player.actionTargets = charactersManager.SearchCharaWithCondition(condition);
        FindObjectOfType<ActionQueueManager>().Enqueue(action_player);
    }
}
