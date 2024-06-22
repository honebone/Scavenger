using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_HarvestScythe : PA_Equipment
{
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    public override void OnKill(List<Action.OnKillParams> onKillParamsList)
    {
        List<Character> target = charactersManager.SearchCharaWithCondition(condition);
        if (target.Count > 0)
        {
            foreach (Action.OnKillParams onKillParams in onKillParamsList)
            {
                if (!onKillParams.obstacle && onKillParams.target.GetCharacterStatus().position >= 9)
                {
                    Enqueue(attack, true, target);
                }
            }
        }
    }
    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += attack.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
