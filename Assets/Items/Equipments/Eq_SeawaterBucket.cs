using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_SeawaterBucket : PA_Equipment
{
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    
    public override void OnRoundEnd()
    {
        List<Character> target = charactersManager.SearchCharaWithCondition(condition);
        if (target.Count > 0)
        {
            Enqueue(attack, true, target);
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += attack.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
