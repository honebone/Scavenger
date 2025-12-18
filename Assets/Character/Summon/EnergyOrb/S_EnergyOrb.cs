using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnergyOrb : PA_Personality
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    public override void OnDie(Character killer)
    {
        Action.ActionStatus action = actionStatus;
        action.INTDMG_divide_int = character.CharaStatus().maxHP;

        Enqueue_SearchTarget(action, condition);
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }
}
