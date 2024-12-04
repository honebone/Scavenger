using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StEU_Werewolf_BloodThirst : PA_StatusEffect
{
    [SerializeField] Character.CharaStatusMod statusMod;
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    [SerializeField] GameObject bleed;


    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        bool f = false;
        if (statusRef.DoesAttack())
        {
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                if (statusRef.actionTargets[i].CheckHasStE(bleed))
                {
                    actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
                    f = true;
                }
            }
        }
        if (!forCalcDMG && f) { AddStack(-1); }
        return actionsStatus;
    }

    public override void OnPAInit()
    {
        character.ModifyStatus(statusMod, true);
    }
    public override void AtTheEnd()
    {
        character.ModifyStatus(statusMod, false);
    }

    //public override string GetAdditionalInfo()
    //{
    //    string s = "";
    //    s += statusMod.GetInfo();
    //    s += "\n" + actionModStatus.GetModInfo();
    //    return s;
    //}

}
