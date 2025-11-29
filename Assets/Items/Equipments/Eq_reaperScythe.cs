using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eq_reaperScythe : PA_Equipment
{
    public int exTurnTH;
    public Action.ActionStatus heal;
    public Action.ActionStatus exTurn;
    int count;
    public override void OnKill(List<Action.OnKillParams> onKillParamsList)
    {
        onKillParamsList.ForEach(x =>
        {
            if (!x.target.IsObstacle())
            {
                count++;
                Enqueue_Self(heal);
                if (count == exTurnTH) Enqueue_Self(exTurn);
                else if(count < exTurnTH) Log($"カウント+1 ({count})");
            }
        });
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        return $"{heal.GetInfo()}\n{exTurn.GetInfo()}";
    }

    public override string GetCurrentStateInfo()
    {
        return count >= exTurnTH ? "追加ターン取得済み" : $"カウント：{count}/{exTurnTH}";
    }
}
