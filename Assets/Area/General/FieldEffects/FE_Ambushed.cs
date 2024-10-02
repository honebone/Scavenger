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
        List<Character> enemyTargets = new List<Character>(charactersManager.SearchCharaWithCondition(condition_enemy));

        action_enemy.actionOwner = null;
        action_enemy.actionTargets = enemyTargets;
        FindObjectOfType<ActionQueueManager>().Enqueue(action_enemy, enemyTargets.Count);

        Action.ActionStatus action_player = actionStatus_player;
        List<Character> playerTargets = new List<Character>(charactersManager.SearchCharaWithCondition(condition_player));

        action_player.actionOwner = null;
        action_player.actionTargets = playerTargets;
        FindObjectOfType<ActionQueueManager>().Enqueue(action_player, playerTargets.Count);
    }
}
