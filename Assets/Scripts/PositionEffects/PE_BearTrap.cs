using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PE_BearTrap : PositionEffect
{
    [SerializeField]
    Action.ActionStatus trap;
    public override void OnCharaEnter()
    {
        Action.ActionStatus action = trap;

        action.actionOwner = null;
        action.actionTargets = new List<Character> { character };
        FindObjectOfType<ActionQueueManager>().Enqueue(action,0);

        AddStack(-1);
    }

    public override string GetAdditionalInfo()
    {
        return trap.GetInfo(false, new Character.CharacterStatus());
    }
}
