using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_General : PA_Equipment
{
    [SerializeField] bool onBS;
    [SerializeField] Action.ActionStatus action_BS;
    [SerializeField] CharactersManager.SearchCharaCondition condition_BS;


    public override void OnBattleStart()
    {
        if (onBS)
        {
            List<Character> target = charactersManager.SearchCharaWithCondition(condition_BS);
            if (target.Count > 0)
            {
                Enqueue(action_BS, true, target);
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += onBS ? action_BS.GetInfo(false, new Character.CharacterStatus()) + "\n" : "";
        return s;
    }
}
