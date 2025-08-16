using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_BountyList : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] int defMax;
    //int maxCount;
    int count;

    void Start()
    {
        //maxCount = defMax;
        count = 0;
    }

    public override void OnFocus(List<Action.OnFocusParams> focusParamsList)
    {
        foreach (var focusParams in focusParamsList)
        {
            if (count < defMax && focusParams.kill)
            {
                count++;
                Enqueue_Self(actionStatus);
                Log($"カウント+1 ({count})");
            }
        }
    }

    //public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    //{
    //    if ( count > 0)
    //    {
    //        Action.ActionStatus action = actionStatus;
    //        action.exTurn = count;
    //        Enqueue_Self(action);
    //        maxCount -= count;
    //        count = 0;
    //    }
    //}

    public override void OnRoundEnd()
    {
        count = 0;
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"このラウンドで得た追加ターン：{count}";
    }
}
