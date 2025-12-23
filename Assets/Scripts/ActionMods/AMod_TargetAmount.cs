using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_TargetAmount : ActionMod
{
    public bool onlyAttack;
    [SerializeField] bool onlyActive;
    [Header("‘ÎÛ”‚±‚ê‚æ‚è‘½‚©‚Á‚½‚ç")] public int TH = 0;
    [SerializeField] bool foreachTarget = true;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (!statusRef.abilityEffect && onlyActive) return actionsStatus;

        if (statusRef.actionTargets.Count > TH && (statusRef.DoesAttack() || !onlyAttack))
        {
            int count = foreachTarget ? statusRef.actionTargets.Count - TH : 1;
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
                }
            }
        }

        return actionsStatus;
    }
}
