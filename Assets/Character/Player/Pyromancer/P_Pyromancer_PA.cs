using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Pyromancer_PA : PA_Personality
{
    [SerializeField, TextArea(3, 10)]
    string info;
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnRoundStart()
    {
        if (character.GetCharacterStatus().CRITC.Dice())
        {
            List<Character> target = charactersManager.SearchCharaWithCondition(condition);
            if (target.Count > 0)
            {
                Enqueue(attack, true, target);
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = info+"\n\n";
        s += attack.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
