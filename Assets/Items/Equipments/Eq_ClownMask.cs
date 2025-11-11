using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_ClownMask : PA_Equipment
{
    [SerializeField]
    int chance;
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (onDamageParams.totalDMG > 0 && chance.Dice())
        {
            Action.ActionStatus action = actionStatus;
            List<Character> targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition));
            targets.Remove(character);
            if (targets.Count > 0)
            {
                actionStatus.actionOwner = character;
                character.Enqueue(action, true, targets);
            }
        }
    }
}
