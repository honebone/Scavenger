using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EqM_Lightning : Eq_Magic
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] int maxRemain = 8;

    int remain;
    public override void OnBattleStart()
    {
        remain = maxRemain;
    }
    public override void OnRoundStart()
    {
        remain = maxRemain;
    }

    public override void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams)
    {
        onApplyStEParams.appliedParams.ForEach(x =>
        {
            if (remain > 0 && x.GetStatusEffectStatus().StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff)
            {
                remain--;
                Cast();
            }
        });
    }

    public override void Cast()
    {
        if (Enqueue_SearchTarget(actionStatus, condition, 1)) character.OnCast(this);
        
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        return $"c‚è‰r¥‰ñ”F{remain}/{maxRemain}";
    }
}
