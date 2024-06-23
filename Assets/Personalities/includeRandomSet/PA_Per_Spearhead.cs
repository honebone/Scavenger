using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Per_Spearhead : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
   
    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }

    public override void OnBattleStart()
    {
        Action.ActionStatus action = actionStatus;
        Enqueue_Self(action);
    }   
}
