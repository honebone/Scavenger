using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Eq_ViciousBow : PA_Equipment
{
    [SerializeField] GameObject scheme;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    public override void OnSomeoneDied(Character died)
    {
        int stack = died.GetStEStack_Sum(scheme);
        if (stack > 0)
        {
            for (int i = 0; i < stack; i++)
            {
                Enqueue_SearchTarget(actionStatus, condition, 1);
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }
}
