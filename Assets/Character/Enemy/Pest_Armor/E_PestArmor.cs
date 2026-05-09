using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_PestArmor : PA_Personality
{
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    [SerializeField] float shieldRatio;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.DoesAttack() && character.CharaStatus().shield > 0)
        {
            int exDMG = (character.CharaStatus().shield * shieldRatio).ToInt();
            actionModStatus.ATKDMG_divide_int = exDMG;
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        actionModStatus.exATKDMG_int = 0;
        return actionsStatus;
    }
    public override string GetPAInfo_Base()
    {
        string s = "";
        s += actionModStatus.GetModInfo();
        return s;
    }
}
