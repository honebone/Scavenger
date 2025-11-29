using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Belief : PA_StatusEffect
{
    //[SerializeField]
    //Action.ActionStatus clearPrayer;
    [SerializeField]
    Action.ActionStatus actionStatus;

    [SerializeField]
    CharactersManager.SearchCharaCondition condition;

    public override void OnPAInit()
    {
        //Action.ActionStatus action = clearPrayer;
        //character.Enqueue(action, true, charactersManager.SearchCharaWithCondition(condition));
    }
    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        if (actionResultsList[0].actionStatus.abilityType != AbilityData.AbilityType.move)
        {
            Enqueue_SearchTarget(actionStatus,condition);
            AddStack(-1);
        }
    }

    //public override void OnTurnEnd(TurnEndParams tep)
    //{
    //    if (tep.actThisTurn && tep.myTurn) { Disable(); }
    //}

    public override string GetAdditionalInfo()
    {
        string s = "";
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
