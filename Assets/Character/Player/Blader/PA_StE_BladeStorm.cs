using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_BladeStorm : PA_StatusEffect
{

    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        List<Character> target = charactersManager.SearchCharaWithCondition(condition);
        Enqueue(attack, true, new List<Character> { target.Choice() });
        AddStack(-1);
    }
}
