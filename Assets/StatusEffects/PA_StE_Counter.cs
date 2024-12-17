using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Counter : PA_StatusEffect
{
    [SerializeField] bool INTCounter;
    [SerializeField]
    Action.ActionStatus actionStatus;
 
    public override void OnAttacked(Character attacker, bool evaded, bool missed)
    {
        if (!character.CharaStatus().dead)
        {
            Action.ActionStatus action = actionStatus;
            if (!INTCounter)
            {
                action.ATKMod_min = StEStatus.value;
                action.ATKMod_max = StEStatus.value;
            }
            else
            {
                action.INTMod_min = StEStatus.value;
                action.INTMod_max = StEStatus.value;
            }
            
            actionStatus.actionOwner = character;
            Enqueue(action, true, new List<Character>() { attacker });

            AddStack(-1);
        }
    }
}
