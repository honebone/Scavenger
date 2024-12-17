using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Surgeon : PA_Personality
{
    [SerializeField] ActionMod.ActionModStatus actionModStatus = new ActionMod.ActionModStatus();
    [SerializeField] float ratio;
    [SerializeField] float exLimitRatio;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.DoesHeal())
        {
            
            Character.CharacterStatus ownerStatus = statusRef.actionOwner.CharaStatus();
            int heal = (ownerStatus.INT * ratio).ToInt();
            float limitRatio = ownerStatus.CRITC + exLimitRatio;

            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                int limit = (statusRef.actionTargets[i].CharaStatus().maxHP * limitRatio / 100f).ToInt();
                Debug.Log($"heal:{heal} limitratio:{limitRatio} limit:{limit} final:{heal.Limit(limit)}");
                actionModStatus.healValue = heal.Limit(limit);
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }

        return actionsStatus;
    }

    public override string GetPAInfo_Base()
    {
        return "";
    }

    public override string GetCurrentStateInfo()
    {
        int heal = (character.CharaStatus().INT * ratio).ToInt();
        float limitRatio = character.CharaStatus().CRITC + exLimitRatio;
        return $"‘‰Á‰ñ•œ—ÊF{heal}\n‘‰Á‰ñ•œ—ÊãŒÀF‘ÎÛ‚ÌmaxHP‚Ì{limitRatio}“";
    }

}
