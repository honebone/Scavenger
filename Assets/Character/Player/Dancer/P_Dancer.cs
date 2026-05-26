using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;

public class P_Dancer : PA_Personality
{
    [SerializeField] int EVDPerShield;
    [SerializeField] int maxEVD;
    [SerializeField] Action.ActionStatus onEvaded;
    [SerializeField] Action.ActionStatus onMoved;

    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        if (onAttackParams.evaded) Enqueue(onEvaded, true, new List<Character> { onAttackParams.actionParams.owner });
    }

    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        if(onMoveParams.passedBy.Count > 0)
        {
            ActionStatus action = onMoved;
            int shield = (Mathf.Min(maxEVD, character.CharaStatus().EVD) / EVDPerShield).ToInt();
            action.shieldPercent_min += shield;
            action.shieldPercent_max += shield;
            Enqueue(action, true, onMoveParams.passedBy);
        }
    }

    public override string GetPAInfo_Base()
    {
        return $"{onEvaded.GetInfo()}\n\n{onMoved.GetInfo()}";
    }
}
