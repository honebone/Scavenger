using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_SpikeShield : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    public override string GetPAInfo_Base()
    {
        string s= actionStatus.GetInfo();
        return s;
    }
    public override void OnRoundStart()
    {
        Action.ActionStatus action = actionStatus;
        actionStatus.actionOwner = character;
        character.Enqueue(action, true, new List<Character>() { character });
    }
}
