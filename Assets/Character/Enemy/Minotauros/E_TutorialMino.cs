using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_TutorialMino : PA_Personality
{
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnDie(Character killer)
    {
        List<Character> target = charactersManager.SearchCharaWithCondition(condition);
        if (target.Count > 0) { Enqueue(attack, true, target); }
    }



   
}
