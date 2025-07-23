using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_DiamondGauntlet : PA_Equipment
{
    public int clearRatio;
    public ActionMod.ActionModStatus actionMod;

    int count;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.DoesAttack() && actionsStatus.Length == 1&& count>0)
        {
            int DMG = (count * clearRatio / 100f).ToInt();
            count -= DMG;
            ActionMod.ActionModStatus mod = actionMod;
            mod.exINTDMG_int = DMG;
            actionsStatus[0] = actionsStatus[0].Modify(mod);
        }

        return actionsStatus;
    }

    public override void OnDecreasedShield(int value)
    {
        count += value;
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}\n次の追加ダメージ：{(count * clearRatio / 100f).ToInt()}";
    }
}
