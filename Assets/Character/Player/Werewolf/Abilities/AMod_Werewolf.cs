using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Werewolf : ActionMod
{
    [SerializeField] int THPercent;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (statusRef.DoesAttack() && statusRef.actionOwner.CharaStatus().GetHPPercent() <= THPercent)
        {
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                ActionModStatus mod = actionModStatus;
                float ratio = THPercent - statusRef.actionOwner.CharaStatus().GetHPPercent();
                mod.drain = ratio;
                ExpeditionRef.infoText.AddDebugText($"drain:{mod.drain}");
                actionsStatus[i] = actionsStatus[i].Modify(mod);
            }
        }
        return actionsStatus;
    }
}
