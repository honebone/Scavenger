using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Hyotan : PA_Equipment
{
    [SerializeField] int EVDPerMove;
    [SerializeField] int maxCount;
    [SerializeField] Action.ActionStatus actionStatus;
    int count;
    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn) { Enqueue_Self(actionStatus); }
    }

    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        if (onMoveParams == null) return;

        if (count < maxCount)
        {
            count++;
            character.AddEVD(EVDPerMove);
            Log($"{"EVD".ToSpr_withName()}+{EVDPerMove} (+{count * EVDPerMove})");
        }
    }

    public override void OnBattleEnd()
    {
        character.AddEVD(-EVDPerMove*count);
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}/{maxCount}\n{"EVD".ToSpr_withName()}+{count * EVDPerMove}";
    }
}
