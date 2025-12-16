using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BattleManager;

public class EqR_Jera : Eq_Rune
{
    public Action.ActionStatus heal;
    public override void OnBattleStart()
    {
        RuneInitialCharge();
    }

    public override void OnKill(List<Action.OnKillParams> onKillParamsList)
    {
        onKillParamsList.ForEach(x =>
        {
            if (!x.target.IsObstacle())
            {
                RuneActivate();
            }
        });
    }

    public override void RuneActivation()
    {
        Enqueue_Self(heal);
    }

    public override void OnBattleEnd()
    {
        ResetRuneCharge();
    }

    public override string GetPAInfo_Base()
    {
        return heal.GetInfo();
    }

    public override string GetCurrentStateInfo()
    {
        return $"チャージ：{runeCharge}";
    }
}
