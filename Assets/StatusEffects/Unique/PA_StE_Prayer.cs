using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Prayer : PA_StatusEffect
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    [SerializeField]
    int stackThreshold;
    public override void OnAddStack(int add)
    {
        if (StEStatus.stack >= stackThreshold)
        {
            Action.ActionStatus action = actionStatus;
            character.Enqueue(action, true, new List<Character>() { character });

            Disable();
        }
    }
    //public override string GetAdditionalInfo()
    //{
    //    return actionStatus.GetInfo(true, character.GetCharacterStatus());
    //}
}
