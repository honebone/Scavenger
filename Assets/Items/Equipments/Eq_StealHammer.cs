using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_StealHammer : PA_Equipment
{
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    [SerializeField] float HPRatio;
    bool activated;
   
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (onDamageParams.totalDMG > 0) { activated = true; }
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
            ActionMod.ActionModStatus mod = actionModStatus;
            mod.exATKDMG_int = (character.CharaStatus().maxHP * HPRatio).ToInt();

            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(mod);
            }
        }

        return actionsStatus;
    }

    public override string GetCurrentStateInfo()
    {
        return activated ? "”\—Í”­“®’†" : "”\—Í–¢”­“®";
    }
}
