using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Regenation : PA_StatusEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn)
        {
            Action.ActionStatus action = actionStatus;
            action.trueHeal = StEStatus.DMGPerTurn;
            Enqueue(action, true, new List<Character>() { character });
            if (StEStatus.applyer != null) { StEStatus.applyer.GetBattleReport().GHeal += StEStatus.DMGPerTurn; }
            AddStack(-1);
        }
    }

    public override string GetCurrentStateInfo()
    {
        string heal = $"次ターン{StEStatus.DMGPerTurn.ToString().ColorStr(Definer.colorRef.heal)}(計{(StEStatus.DMGPerTurn * StEStatus.stack).ToString().ColorStr(Definer.colorRef.heal)})";
        return $"回復量：{heal}";
    }
}
