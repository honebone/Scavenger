using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_UnderSleeve : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        if (actionResultsList[0].actionStatus.abilityType != AbilityData.AbilityType.attack)
        {
            Action.ActionStatus action = actionStatus;
            List<Character> target = charactersManager.SearchCharaWithCondition(condition);
            Enqueue(action, true, new List<Character> { target.Choice() });
        }
    }
}
