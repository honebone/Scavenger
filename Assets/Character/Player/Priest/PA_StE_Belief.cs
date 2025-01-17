using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Belief : PA_StatusEffect
{
    [SerializeField]
    Action.ActionStatus clearPrayer;
    [SerializeField]
    Action.ActionStatus actionStatus;

    [SerializeField]
    CharactersManager.SearchCharaCondition condition;

    bool activateAbility;

    public override void OnPAInit()
    {
        Action.ActionStatus action = clearPrayer;
        character.Enqueue(action, true, charactersManager.SearchCharaWithCondition(condition));
    }
    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        Action.ActionStatus action = actionStatus;
        character.Enqueue(action, true, charactersManager.SearchCharaWithCondition(condition));

        activateAbility = true;
    }

    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (activateAbility&&myTurn) { Disable(); }
    }

    public override string GetAdditionalInfo()
    {
        string s = "";
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        s += "自身が行動したターンの終了時、これを消去";
        return s;
    }
}
