using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Per_HuntersInstinct : PA_Personality
{
    [SerializeField] int ACCTH;
    [SerializeField] float CRITCPerACC;
    [SerializeField] float CRITDPerACC;
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (!statusRef.DoesAttack() || !statusRef.abilityEffect || character.CharaStatus().ACC < ACCTH) return actionsStatus;
        float count = character.CharaStatus().ACC - ACCTH;

        ActionMod.ActionModStatus actionMod = actionModStatus;
        actionMod.CRITCMod += CRITCPerACC * count;
        actionMod.CRITDMod += CRITDPerACC * count;

        for(int i = 0; i < actionsStatus.Count(); i++)
        {
            actionsStatus[i] = actionsStatus[i].Modify(actionMod);
        }

        return actionsStatus;
    }

    public override string GetPAInfo_Base()
    {
        return actionModStatus.GetModInfo();
    }
}
