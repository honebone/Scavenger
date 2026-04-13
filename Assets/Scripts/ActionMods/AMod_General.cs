using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_General : ActionMod
{
    [SerializeField] bool onlyAttack;
    [SerializeField] bool onlyActive;
    [SerializeField] bool onlyPassive;


    [SerializeField, Header("\n\n”­“®Ò‚ÌğŒ")]
    bool hasCondition_owner;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition_owner;


    [SerializeField, Header("\n\n‘ÎÛ‚ÌğŒ")]
    bool hasCondition_target;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition_target;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (onlyAttack && !statusRef.DoesAttack()) return actionsStatus;
        if (onlyActive && !statusRef.abilityEffect) return actionsStatus;
        if (onlyPassive && statusRef.abilityEffect) return actionsStatus;

        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if (!hasCondition_owner || charactersManager.ExamineCharacter(statusRef.actionOwner, condition_owner))//”­“®Ò‚ÌğŒ
            {
                if (!hasCondition_target || charactersManager.ExamineCharacter(statusRef.actionTargets[i], condition_target))//‘ÎÛ‚ÌğŒ
                {
                    actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
                }
            }
        }
        return actionsStatus;
    }
}
