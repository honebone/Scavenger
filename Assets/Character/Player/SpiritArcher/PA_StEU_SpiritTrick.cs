using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;
using static ActionMod;

public class PA_StEU_SpiritTrick : PA_StatusEffect
{
    public ActionMod.ActionModStatus actionMod;
    public Action.ActionStatus actionStatus;

    // Start is called before the first frame update
    public override Action.ActionStatus ModifyAction_Targeted(Action.ActionStatus statusRef, bool forCalcDMG)
    {
        if (statusRef.abilityEffect && statusRef.DoesAttack())
        {
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                statusRef = statusRef.Modify(actionMod);
            }
        }
        return statusRef;
    }

    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (onDamageParams.ap.actionStatus.abilityEffect) { AddStack(-1); }
    }

    public override string GetPAInfo_Base()
    {
        return actionMod.GetModInfo();
    }
}
