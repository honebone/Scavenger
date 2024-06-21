using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Priest_Purify : Ability
{
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;
    public override Action.ActionStatus ModifyTargetParams(Action.ActionStatus actionStatus)
    {
        if (charactersManager.CheckIfMatchCondition(character, condition))
        {
            actionStatus.targetType = Action.ActionStatus.TargetType.column;
        }
        return actionStatus;
    }
}
