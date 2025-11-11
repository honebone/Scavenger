using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StEU_Star_FearEndless : PA_StatusEffect
{
    public int removeTH;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (character.CharaStatus().GetHPPercent().ToInt() >= removeTH) { Disable(); }
        else if (tep.myTurn && applyFlag)
        {
            AddStack(-1);
        }
    }

    public override void AtTheEnd()
    {
        if (BattleManager.inBattle && StEStatus.stack == 0)
        {
            Enqueue_SearchTarget(actionStatus, condition);
        }
    }

    public override string GetCurrentStateInfo()
    {
        return $"‰ğœ‚É•K—v‚È{"HP".ToSpr_withName()}F{(character.CharaStatus().maxHP * removeTH / 100f).ToInt()}";
    }

    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo();
    }
}
