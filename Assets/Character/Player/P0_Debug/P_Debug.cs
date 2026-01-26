using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Debug : PA_Personality
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    public override void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams)
    {
        Enqueue_SearchTarget(actionStatus, condition);
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }
}
