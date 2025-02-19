using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_BladeStorm : PA_StatusEffect
{

    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition_focus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    //public override void OnMoved(Action.OnMoveParams onMoveParams)
    //{
    //    List<Character> target_focus = charactersManager.SearchCharaWithCondition(condition_focus);
    //    List<Character> target = charactersManager.SearchCharaWithCondition(condition);
    //    if (target_focus.Count > 0) { Enqueue(attack, true, target_focus,1); }
    //    else if (target.Count > 0) { Enqueue(attack, true, target,1); }
    //    AddStack(-1);
    //}
}
