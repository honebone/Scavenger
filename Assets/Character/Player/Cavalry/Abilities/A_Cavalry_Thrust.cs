using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Cavalry_Thrust : Ability
{
    public CharactersManager.SearchCharaCondition condition;
    public override Action.ActionStatus ModifyTargetParams(Action.ActionStatus actionStatus)
    {
        if (charactersManager.CheckIfMatchCondition(character, condition))
        {
            actionStatus.targetType = Action.ActionStatus.TargetType.row;
        }
        return actionStatus;
    }
}
