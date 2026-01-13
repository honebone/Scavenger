using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_BloodyMask : PA_Equipment
{
    public int healPercent;
    public int count;
    public Action.ActionStatus actionStatus;
    public override void OnDecreasedHP(int value)
    {
        if (BattleManager.inst.checkIfMyTurn(character))
        {
            count += value;
            Log($"カウント+{value} ({count})");
        }
    }

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (count > 0 && tep.myTurn) {
            Action.ActionStatus action = actionStatus;
            action.trueHeal = (count * healPercent / 100f).ToInt();
            Enqueue_Self(action);
            count = 0;
        }
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count} (回復量{(count * healPercent / 100f).ToInt()})";
    }
}
