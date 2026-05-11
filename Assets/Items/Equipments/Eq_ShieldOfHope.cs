using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_ShieldOfHope : PA_Equipment
{
    [SerializeField] int shieldMul;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnAddedShield(int value, Action.ActionParams actionParams)
    {
        Action.ActionStatus action = actionStatus;
        int shield = value.Mul(shieldMul);
        action.shieldAdd_max = shield;
        action.shieldAdd_min = shield;
        Enqueue_SearchTarget(action, condition, 1);
    }
}
