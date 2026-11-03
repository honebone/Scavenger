using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionMod;

public class PA_StEU_Miko_Fuma : PA_StatusEffect
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] Action.ActionStatus ClearCD;
    [SerializeField] CharactersManager.SearchCharaCondition miko;
    [SerializeField] ActionModStatus actionMod;

    public override void OnPAInit()
    {
        Enqueue_SearchTarget(actionStatus, condition);
    }

    public override Action.ActionStatus ModifyAction_Targeted(Action.ActionStatus statusRef, bool forCalcDMG)
    {
        if (!statusRef.DoesAttack()) return statusRef;

        statusRef = statusRef.Modify(actionMod);

        return statusRef;
    }

    public override void OnDie(Character killer)
    {
        Enqueue_SearchTarget(ClearCD, miko);
    }

    public override string GetAdditionalInfo()
    {
        return ClearCD.GetInfo();
    }
}
