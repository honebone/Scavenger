using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Slime_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    List<int> list= new List<int>() { 9,10,11,12,13,14,15,16,17};
    List<int> empty=new List<int>();
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (33.Dice())
        {
            Action.ActionStatus action = actionStatus;
            actionStatus.actionOwner = character;
            empty = charactersManager.GetEmptyPos(list);
            action.actionTargetsInt = new List<int> { empty[Random.Range(0, empty.Count)] };
            character.Enqueue(action, false, new List<Character>());
        }
    }
   
    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo(true, character.GetCharacterStatus());
    }
}
