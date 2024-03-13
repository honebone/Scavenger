using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FE_Ambushed : FieldEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus_enemy;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition_enemy;
    [SerializeField]
    Action.ActionStatus actionStatus_player;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition_player;

    public override string GetFEInfo()
    {
        return "error";
    }
    public override void OnBattleStart()
    {
        Action.ActionStatus action_enemy = actionStatus_enemy;

        action_enemy.actionOwner = null;
        action_enemy.actionTargets = charactersManager.SearchCharaWithCondition(condition_enemy);
        FindObjectOfType<ActionQueueManager>().Enqueue(action_enemy);

        Action.ActionStatus action_player = actionStatus_player;

        action_player.actionOwner = null;
        action_player.actionTargets = charactersManager.SearchCharaWithCondition(condition_player);
        FindObjectOfType<ActionQueueManager>().Enqueue(action_player);
    }
}
