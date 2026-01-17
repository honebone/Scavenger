using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_RoughShield : PA_Personality
{
    [SerializeField] int ratio;
    [SerializeField] ActionMod.ActionModStatus modStatus;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        int shield = character.CharaStatus().shield;
        if (statusRef.DoesAttack() && statusRef.abilityEffect&& shield>0)
        {
            ActionMod.ActionModStatus mod = modStatus;
            mod.ATKDMG_divide_int = shield.Mul(ratio);
            for (int i = 0;i<actionsStatus.Length;i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(mod);
            }
        }

        return actionsStatus;
    }

    public override string GetPAInfo_Base()
    {
        return modStatus.GetModInfo();
    }
}
