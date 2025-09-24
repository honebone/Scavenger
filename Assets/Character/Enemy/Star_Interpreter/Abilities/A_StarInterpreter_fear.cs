using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class A_StarInterpreter_fear : Ability
{
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override List<List<int>> GetTargetPool(int index)
    {
        List<Character> pool = new List<Character>();
        List<List<int>> pools = new List<List<int>>();
        if (index == 0)
        {
            pool = charactersManager.SearchChara_Weakest(condition, true);
            pools = pool.Select(m => new List<int> { m.CharaStatus().position }).ToList();
        }
        if (index == 1)
        {
            pool = charactersManager.SearchChara_Strongest(condition, true);
            pools = pool.Select(m => new List<int> { m.CharaStatus().position }).ToList();
        }

        return pools;
    }
}
