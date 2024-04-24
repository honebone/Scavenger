using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_PoundingHeart : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    public override string GetPAInfo()
    {
        return actionStatus.GetInfo(false,new Character.CharacterStatus());
    }
    public override void OnRoundStart()
    {
        Action.ActionStatus action = actionStatus;
        actionStatus.actionOwner = character;
        character.Enqueue(action, true, new List<Character>() { character });
    }
}
