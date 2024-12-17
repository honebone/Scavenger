using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_RatKing_Stare : Ability
{
    [SerializeField] GameObject head;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    public override List<List<int>> GetTargetPool(int index)
    {
        int targetCount = 1 + character.GetStEStack_Sum(head);
        List<Character> target = charactersManager.SearchCharaWithCondition(condition).Sample(targetCount);
        List<int> pos = new List<int>();
        foreach(Character c in target)
        {
            pos.Add(c.CharaStatus().position);
        }

        return new List<List<int>> { pos };
    }
}
