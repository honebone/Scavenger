using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_RgRobber_Stab : Ability
{
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override List<List<int>> GetTargetPool(int index)
    {
        List<Character> pool = charactersManager.SearchCharaWithCondition(condition);
       float lowestHPPercent = 100f;
        foreach (Character chara in pool)
        {
            Character.CharacterStatus status = chara.CharaStatus();
            if (status.GetHPPercent() < lowestHPPercent) { lowestHPPercent = status.GetHPPercent();}
        }

        List<Character> pool2 = new List<Character>();
        foreach (Character chara in pool)
        {
            if (chara.CharaStatus().GetHPPercent() == lowestHPPercent) { pool2.Add(chara); }
        }

        return new List<List<int>>() { new List<int> { pool2.Choice().CharaStatus().position } };
    }
}
