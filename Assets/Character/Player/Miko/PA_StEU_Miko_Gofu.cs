using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StEU_Miko_Gofu : PA_StatusEffect
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnPAInit()
    {
        Enqueue_SearchTarget(actionStatus,condition);
    }
}
