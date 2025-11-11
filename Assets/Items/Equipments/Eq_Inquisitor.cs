using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Inquisitor : PA_Equipment
{
    public ActionMod.ActionModStatus actionModStatus;
    bool activated;

    public override void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams)
    {
        foreach(var applied in onApplyStEParams.appliedParams)
        {
            if(applied.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff)
            {
                if (!activated) { Log("”­“®"); };
                activated = true;
                return;
            }
        }
    }

    public override void OnBattleEnd()
    {
        activated = false;
    }

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (activated && statusRef.DoesAttack())
        {
            if (!forCalcDMG) { activated = false; }

            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }

        return actionsStatus;
    }

    public override string GetPAInfo_Base()
    {
        string s = actionModStatus.GetModInfo();
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        return activated ? "”\—Í”­“®’†" : "”\—Í–¢”­“®";
    }
}
