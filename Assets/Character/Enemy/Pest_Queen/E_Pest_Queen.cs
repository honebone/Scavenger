using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Pest_Queen : PA_Personality
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] int countPerExTurn;
    int count;

    public override void OnSomeoneDied(Character died)
    {
        if (!died.CharaStatus().position.IsPlayerPos() && !died.CharaStatus().Obstacle())
        {
            count++;
            Log($"カウント+1 ({count})");
        }
    }

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn)
        {
            Action.ActionStatus action = actionStatus;
            while (count >= countPerExTurn)
            {
                action.exTurn++;
                count -= countPerExTurn;
            }


            if(action.exTurn>0) Enqueue_Self(action);
        }
    }

    public override string GetCurrentStateInfo()
    {
        return $"現在カウント：{count}";
    }
}
