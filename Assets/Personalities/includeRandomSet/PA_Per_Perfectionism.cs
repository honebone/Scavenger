using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Per_Perfectionism : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField]
    int chance;

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }
   
    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        foreach (Action.OnAttackParams param in onAttackParamsList)
        {
            if (param.evaded && chance.Dice())
            {
                Action.ActionStatus action = actionStatus;
                actionStatus.actionOwner = character;
                character.Enqueue(action, true, new List<Character>() { character });
            }
        }
    }
}
