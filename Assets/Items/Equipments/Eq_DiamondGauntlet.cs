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
        if (statusRef.DoesAttack() && statusRef.abilityEffect && count > 0)
        {
            int exDMG = (count * clearRatio / (actionsStatus.Length * 100f)).ToInt();
            if (exDMG > 0)
            {
                count -= exDMG * actionsStatus.Length;

                ActionMod.ActionModStatus mod = actionMod;
                mod.exINTDMG_int = exDMG;

                for (int i = 0; i < actionsStatus.Length; i++) { actionsStatus[i] = actionsStatus[i].Modify(mod); }
            }
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
