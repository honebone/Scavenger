using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Quiver : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    bool act;
    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }
   
    public override void OnActivateAbility()
    {
        act = true;
    }
    public override void OnTurnEnd()
    {
        if (!act)
        {
            Action.ActionStatus action = actionStatus;
            actionStatus.actionOwner = character;
            character.Enqueue(action, true, new List<Character>() { character });
        }
        act = false;
    }
}
