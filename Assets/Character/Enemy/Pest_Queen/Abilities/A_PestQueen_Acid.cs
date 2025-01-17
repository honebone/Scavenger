using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_PestQueen_Acid : Ability
{
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override List<List<int>> GetTargetPool(int index)
    {

        return new List<List<int>>() { new List<int>(charactersManager.SearchPosWithCondition(condition).Sample(3)) };
    }
}
