using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_BountyList : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] int defMax;
    int maxCount;
    int count;

    public override void OnFocus(List<Action.OnFocusParams> focusParamsList)
    {
        foreach(var focusParams in focusParamsList)
        {
            if (count < maxCount&&focusParams.kill)
            {
                count++;
                Log($"ƒJƒEƒ“ƒg‘‰Á({count})");
            }
        }
    }

    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn)
        {

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
}
