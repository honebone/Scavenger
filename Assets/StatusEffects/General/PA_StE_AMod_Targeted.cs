using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;
using static ActionMod;

public class PA_StE_AMod_Targeted : PA_StatusEffect
{
    [SerializeField] bool onlyAttack;
    [SerializeField] bool onlyAbility;
    [SerializeField] bool onlyPassive;
    [SerializeField,Header("スタック数だけ繰り返すか")] bool mulByStack;
    [SerializeField] ActionModStatus actionMod;

    public override Action.ActionStatus ModifyAction_Targeted(Action.ActionStatus statusRef, bool forCalcDMG)
    {
        if (!statusRef.DoesAttack() && onlyAttack) return statusRef;
        if (!statusRef.abilityEffect && onlyAbility) return statusRef;
        if (statusRef.abilityEffect && onlyPassive) return statusRef;

        int count = mulByStack ? StEStatus.stack : 1;
        for(int i = 0; i < count; i++)
        {
            statusRef = statusRef.Modify(actionMod);
        }

        return statusRef;
    }

    public override string GetAdditionalInfo()
    {
        return actionMod.GetModInfo();
    }
}
