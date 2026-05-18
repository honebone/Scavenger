using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StEU_Star_FearEndless : PA_StatusEffect
{
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;

    public override void OnRoundEnd()
    {
        Enqueue_SearchTarget(actionStatus, condition);
    }

    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        AddStack(-1);
    }

    public override string GetCurrentStateInfo()
    {
        return "アビリティを発動するとスタックを減少できる";
    }

    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo();
    }
}
