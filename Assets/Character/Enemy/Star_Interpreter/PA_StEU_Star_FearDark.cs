using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;
using System.Linq;

public class PA_StEU_Star_FearDark : PA_StatusEffect
{
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn && !tep.actThisTurn)
        {
            AddStack(-1);
        }
    }

    public override void OnRoundEnd()
    {
        Enqueue_SearchTarget(actionStatus,condition);
    }

    public override string GetCurrentStateInfo()
    {
        return "ターンをパスするとスタックを減少できる";
    }

    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo();
    }
}
