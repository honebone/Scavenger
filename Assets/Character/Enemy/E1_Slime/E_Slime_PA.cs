using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Slime_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    public override void OnDamaged(int DMG, Character attacker)
    {
        if (50.Probability())
        {
            Action.ActionStatus action = actionStatus;
            actionStatus.actionOwner = character;
            character.Enqueue(action, true, new List<Character>() { attacker });
        }
    }
   
    public override string GetPAInfo()
    {
        return actionStatus.GetInfo(true, character.GetCharacterStatus());
    }
}
