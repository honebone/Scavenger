using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StEU_Sniper_Dynamite : PA_StatusEffect
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        AddStack(-1);
        //if (onDamageParams.ap.owner == StEStatus.applyer)
        //{
        //    AddStack(-1);
        //}
    }

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn) AddStack(-1);
    }

    public override void AtTheEnd()
    {
        if (BattleManager.inBattle && StEStatus.applyer != null && StEStatus.applyer.CheckAlive())
        {
            Action.ActionStatus action = actionStatus;
            action.ATKMod_divide = StEStatus.value;

            StEStatus.applyer.Enqueue(action, true, charactersManager.SearchCharaWithCondition(condition,character));
        }
    }

    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo();
    }
    
}
