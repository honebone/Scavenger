using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Comannder : PA_Personality
{
    [SerializeField] Action.ActionStatus operation;
    [SerializeField] CharactersManager.SearchCharaCondition focusSearch;
    [SerializeField] Action.ActionStatus focus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    public override void OnSomeoneFocus(List<Action.OnFocusParams> focusParamsList)
    {
        for(int i = 0; i < focusParamsList.Count; i++) { Enqueue_Self(operation); }
        foreach(Action.OnFocusParams onFocusParams in focusParamsList)
        {
            if (onFocusParams.actionParams.owner.PlayerPos() == character.PlayerPos()) { Enqueue_Self(operation); }
        } 
    }

    public override void OnRoundEnd()
    {
        if (charactersManager.SearchCharaWithCondition(focusSearch).Count == 0)
        {
            List<Character> targets = charactersManager.SearchChara_Weakest(condition, true);
            Enqueue(focus, true, targets, 1);
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += operation.GetInfo(false, new Character.CharacterStatus());
        s += "\n"+focus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
