using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionMod;

public class EqR_Nauthiz : Eq_Rune
{
   public ActionMod.ActionModStatus actionMod;
    int count;
    public override void OnBattleStart()
    {
        RuneInitialCharge();
        count = 0;
    }

    public override void OnApplyStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        if (onApplyStEParamsList.Any(x => x.appliedParams.Any(y => y.GetStEType() == PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff))) RuneActivate();
    }

    public override void RuneActivation()
    {
        count++;
    }

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (count>0&&statusRef.DoesAttack() && statusRef.abilityEffect)
        {
           
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    actionsStatus[i] = actionsStatus[i].Modify(actionMod);
                }
            }

            if (!forCalcDMG)
            {
                count = 0;
            }
        }

        return actionsStatus;
    }

    public override void OnBattleEnd()
    {
        ResetRuneCharge();
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = actionMod.GetModInfo();
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return $"チャージ：{runeCharge}\nカウント：{count}";
    }
}
