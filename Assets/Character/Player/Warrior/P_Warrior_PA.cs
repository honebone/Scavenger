using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Warrior_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    public override void OnDecreasedHP(int value)
    {
        Action.ActionStatus action = actionStatus;
        Enqueue_Self(action);
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
