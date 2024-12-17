using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_Pyromancer : ActionMod
{
    [SerializeField] GameObject burn;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
    

        Character.CharacterStatus ownerStatus = statusRef.actionOwner.CharaStatus();
        StEApplyBonus bonus = new StEApplyBonus();
        bonus.applyStE = burn;
        bonus.exChance += ownerStatus.CRITC;
        actionModStatus.applyStEBonus = new List<StEApplyBonus>() { bonus };

        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
        }

        return actionsStatus;
    }
}
