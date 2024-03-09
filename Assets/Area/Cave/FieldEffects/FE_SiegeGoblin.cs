using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FE_SiegeGoblin : FieldEffect
{
    [SerializeField]
    int interval;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;
    [SerializeField]
    Action.ActionStatus action;

    public override string GetFEInfo()
    {
        return action.GetInfo(false, new Character.CharacterStatus());
    }
    public override void OnTurnStart(int turnCount)
    {
        if (turnCount % interval == 1)
        {
            Action.ActionStatus act = action;

            List<Character> targetPool = new List<Character>(charactersManager.SearchCharaWithCondition(condition));
            act.actionTargets = targetPool.Sample(2);
            FindObjectOfType<ActionQueueManager>().Enqueue(act);
        }
    }
}
