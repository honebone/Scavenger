using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ability_Weakest : Ability
{
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override List<List<int>> GetTargetPool(int index)
    {
        List<Character> pool = charactersManager.SearchChara_Weakest(condition, true);
        List<List<int>> pools = pool.Select(m => new List<int> { m.CharaStatus().position }).ToList();

        return pools;
    }
}
