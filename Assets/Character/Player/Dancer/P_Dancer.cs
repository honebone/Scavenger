using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;
using static PA_StatusEffect;

public class P_Dancer : PA_Personality
{
    [SerializeField] int EVDPerShield;
    [SerializeField] int maxEVD;
    [SerializeField] int burnPerEVD;
    [SerializeField] Action.ActionStatus onEvaded;
    [SerializeField] Action.ActionStatus onMoved;

    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        if (onAttackParams.evaded)
        {
            ActionStatus action = onEvaded;
            List<StatusEffectParams> list = new List<StatusEffectParams>(action.applySteParams);
            StatusEffectParams burn = list[1];
            int EVD = Mathf.Min(maxEVD, character.CharaStatus().EVD).ToInt();
            burn.value += EVD * burnPerEVD;

            list[1] = burn;
            action.applySteParams = new List<StatusEffectParams>(list);

            Enqueue(action, true, new List<Character> { onAttackParams.actionParams.owner });
        }
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
