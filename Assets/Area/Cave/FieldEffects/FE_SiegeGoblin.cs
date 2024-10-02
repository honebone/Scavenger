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
            infoText.AddDebugText(string.Format("turncount:{0} interval:{1} o:{2}", turnCount, interval, turnCount % interval));
            Action.ActionStatus act = action;

            List<Character> targetPool = new List<Character>(charactersManager.SearchCharaWithCondition(condition));
            act.actionTargets = targetPool;
            FindObjectOfType<ActionQueueManager>().Enqueue(act, 2);
        }
    }
}
