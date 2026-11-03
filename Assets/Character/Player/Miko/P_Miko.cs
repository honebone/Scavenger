using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Miko : PA_Personality
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] GameObject Gofu;
    [SerializeField] int maxCount;
    int remain;
    public override void OnBattleStart()
    {
        remain = maxCount;
    }

    public override void OnTurnStart2(TurnStartParams tsp)
    {
        if (remain == 0) return;
        if(!tsp.turnChara.CheckHasStE(Gofu))return;
        Enqueue(actionStatus,true,new List<Character> { tsp.turnChara });   
        remain--;
    }

    public override void OnRoundStart()
    {
        remain = maxCount;
    }

    public override string GetCurrentStateInfo()
    {
        return $"écÇËî≠ìÆâÒêî{remain}/{maxCount}";
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }
}
