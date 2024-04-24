using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Sister_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    public override void OnActivateAbility()
    {
        Action.ActionStatus action = actionStatus;
        character.Enqueue(action, true, new List<Character>() { character });
    }

    public override void OnHeal(List<Action.OnHealParams> onHealParamsList)
    {
        Action.ActionStatus action = actionStatus;
        character.Enqueue(action, true, new List<Character>() { character });
    }

    public override string GetPAInfo()
    {
        return actionStatus.GetInfo(true, character.GetCharacterStatus());
    }
}
