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
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }
    public override void OnDamaged(int DMG, Character attacker)
    {
        if (chance.Probability())
        {
            Action.ActionStatus action = actionStatus;
            List<Character> targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition));
            targets.Remove(character);
            actionStatus.actionOwner = character;
            character.Enqueue(action, true, targets);
        }
    }
}
