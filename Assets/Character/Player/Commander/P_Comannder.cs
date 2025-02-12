using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Comannder : PA_Personality
{
    [SerializeField] Action.ActionStatus operation;
    [SerializeField] Action.ActionStatus onFocus;
    [SerializeField] CharactersManager.SearchCharaCondition focusSearch;
    [SerializeField] Action.ActionStatus focus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    public override void OnBattleStart()
    {
        Enqueue_Self(operation);
    }
    public override void OnSomeoneFocus(List<Action.OnFocusParams> focusParamsList)
    {
        foreach (Action.OnFocusParams onFocusParams in focusParamsList)
        {
            if (onFocusParams.actionParams.owner.PlayerPos() == character.PlayerPos()) { Enqueue_Self(onFocus); }
        }
    }

    public override void OnRoundEnd()
    {
        infoText.AddDebugText(charactersManager.SearchCharaWithCondition(focusSearch).Count.ToString());
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
        s += "\n" + onFocus.GetInfo(false, new Character.CharacterStatus());
        s += "\n" + focus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
