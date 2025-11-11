using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;

public class PA_StEU_Star_Wish : PA_StatusEffect
{
    public int removeTH;
    int THDMG;
    int currentDMG;

    public Action.ActionStatus actionStatus;

    public override void OnDecreasedHP(int value)
    {
        currentDMG += value;
        if (currentDMG >= (character.CharaStatus().maxHP*removeTH / 100f).ToInt()) { Disable(); }
    }

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn && applyFlag)
        {
            AddStack(-1);
        }
    }

    public override void AtTheEnd()
    {
        if (BattleManager.inBattle && StEStatus.stack == 0)
        {
            Enqueue_Self(actionStatus);
        }
    }

    public override string GetCurrentStateInfo()
    {
        return $"å∏è≠ÇµÇΩ{"HP".ToSpr_withName()}ÅF{currentDMG}/{(character.CharaStatus().maxHP * removeTH / 100f).ToInt()}";
    }
    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo();
    }
}
