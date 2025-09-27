using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;
using System.Linq;

public class PA_StEU_Star_FearDark : PA_StatusEffect
{
    public int removeTH;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;

    int count;

    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn && applyFlag)
        {
            AddStack(-1);
        }
    }

    public override void OnApplyedStE(OnApplyStEParams onApplyStEParams)
    {
        count += onApplyStEParams.appliedParams.Where(m => m.GetStatusEffectStatus().StEType == StatusEffectStatus.StatusEffectType.buff).Count();
        if (count >= removeTH) Disable();
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
        return $"‰ğœ‚É•K—v‚È{"buff".ToSpr_withName()}‰ñ”F{count}/{removeTH}";
    }

    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo();
    }
}
