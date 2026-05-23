using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionMod;

public class E_StarServant2 : PA_Personality
{
    [SerializeField] ActionModStatus actionModStatus;

    public override Action.ActionStatus ModifyAction_Targeted(Action.ActionStatus statusRef, bool forCalcDMG)
    {
        if (!statusRef.DoesAttack()) return statusRef;
        if(statusRef.actionOwner.CharaStatus().position.GetColumn()==character.CharaStatus().position.GetColumn())statusRef=statusRef.Modify(actionModStatus);
        return statusRef;
    }

    public override string GetPAInfo_Base()
    {
        return actionModStatus.GetModInfo();
    }
}
