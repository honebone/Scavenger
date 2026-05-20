using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_DropKick : PA_Personality
{
    [SerializeField] int ratio;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        if(onMoveParams.dir == 0)
        {
            Action.ActionStatus action = actionStatus;
            action.ATKDMG_divide_int = character.CharaStatus().maxHP.Mul(ratio);

            Enqueue_SearchTarget(action,condition);
        }
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }
}
