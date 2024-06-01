using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Counter : PA_StatusEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus;
 
    public override void OnAttacked(Character attacker, bool evaded, bool missed)
    {
        if (!character.GetCharacterStatus().dead)
        {
            Action.ActionStatus action = actionStatus;
            action.ATKMod_min = StEStatus.value;
            action.ATKMod_max = StEStatus.value;
            actionStatus.actionOwner = character;
            character.Enqueue(action, true, new List<Character>() { attacker });

            AddStack(-1);
        }
    }
}
