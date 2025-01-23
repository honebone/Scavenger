using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_BountyList : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] int defMax;
    int maxCount;
    int count;

    void Start()
    {
        maxCount = defMax;
        count = 0;
    }

    public override void OnFocus(List<Action.OnFocusParams> focusParamsList)
    {
        foreach (var focusParams in focusParamsList)
        {
            if (count < maxCount && focusParams.kill)
            {
                count++;
                Log($"カウント増加({count})");
            }
        }
    }

    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if ( count > 0)
        {
            Action.ActionStatus action = actionStatus;
            action.exTurn = count;
            Enqueue_Self(action);
            maxCount -= count;
            count = 0;
        }
    }

    public override void OnRoundEnd()
    {
        maxCount = defMax;
        count = 0;
    }

    public override void OnBattleEnd()
    {
        maxCount = defMax;
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"このラウンドで得た追加ターン：{defMax - maxCount}";
    }
}
