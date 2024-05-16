using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Hunter_Hunt : ActionMod
{
    [SerializeField]
    float maxATKMod;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        Character.CharacterStatus ownerStatus = statusRef.actionOwner.GetCharacterStatus();
        actionModStatus.ATKMod = Mathf.Clamp(ownerStatus.ACC - 100, 0, maxATKMod) * 2;
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
        }
        return actionsStatus;
    }
}
