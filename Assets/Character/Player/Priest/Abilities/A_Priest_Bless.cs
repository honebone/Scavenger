using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Priest_Bless : Ability
{
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;
    public override Action.ActionStatus ModifyTargetParams(Action.ActionStatus actionStatus)
    {
        if (charactersManager.ExamineCharacter(character, condition))
        {
            actionStatus.targetType = Action.ActionStatus.TargetType.all;
        }
        return actionStatus;
    }
}
