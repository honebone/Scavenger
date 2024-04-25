using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Belief : PA_StatusEffect
{

    [SerializeField]
    Action.ActionStatus actionStatus;

    [SerializeField]
    CharactersManager.SearchCharaCondition condition;

    public override void OnActivateAbility()
    {
        Action.ActionStatus action = actionStatus;
        character.Enqueue(action, true, charactersManager.SearchCharaWithCondition(condition));

        Disable();
    }

    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }
}
