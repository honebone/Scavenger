using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_General : ActionMod
{
    [SerializeField, Header("”­“®Ò‚ÌğŒ")]
    bool hasCondition_owner;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition_owner;
    [SerializeField, Header("\n\n‘ÎÛ‚ÌğŒ")]
    bool hasCondition_target;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition_target;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if (!hasCondition_owner || charactersManager.CheckIfMatchCondition(statusRef.actionOwner, condition_owner))//”­“®Ò‚ÌğŒ
            {
                if (!hasCondition_target || charactersManager.CheckIfMatchCondition(statusRef.actionTargets[i], condition_target))//‘ÎÛ‚ÌğŒ
                {
                    Debug.Log("ok");
                    actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
                    Debug.Log($"{actionsStatus[i].removeStE_debuff},{actionsStatus[i].removeStE_buff}");

                }
            }
        }
        return actionsStatus;
    }
}
