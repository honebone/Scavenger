using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_VesselOfWish : PA_Equipment
{
    public int countTH;
    [SerializeField] Action.ActionStatus action;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    int count;
    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (count < countTH && tep.turnChara.IsPlayer() && !tep.actThisTurn)
        {
            count++;
            Log($"カウント+1 ({count})");
            if (count == countTH)
            {
                Enqueue_SearchTarget(action, condition,1);
            }
        }
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = action.GetInfo();
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        return count == countTH ? "能力発動済み" : $"カウント：{count}/{countTH}";
    }
}
