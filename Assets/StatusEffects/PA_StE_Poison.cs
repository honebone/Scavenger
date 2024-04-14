using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Poison : PA_StatusEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    public override void OnActivateAbility()
    {
        Action.ActionStatus action = actionStatus;
        action.decreaseHP_min = StEStatus.stack;
        action.decreaseHP_max = StEStatus.stack;
        character.Enqueue(action, true, new List<Character>() { character });
        AddStack(-2);
    }
    
}
