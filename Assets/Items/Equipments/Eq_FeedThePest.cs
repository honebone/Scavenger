using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_FeedThePest : PA_Equipment
{
    public int usePerRound;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public Action.ActionStatus actionStatus_self;

    int remain;
    public override void OnBattleStart()
    {
        remain = usePerRound;
    }

    public override void OnRoundStart()
    {
        remain = usePerRound;
    }

    public override void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams)
    {
        if (remain > 0)
        {
            foreach (var applied in onApplyStEParams.appliedParams)
            {
                if (applied.GetStatusEffectStatus().StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff)
                {
                    remain--;
                    Enqueue_Self(actionStatus_self);
                    Enqueue_SearchTarget(actionStatus, condition, 1);
                    if (remain == 0) break;
                }
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus_self.GetInfo() + "\n";
        s += actionStatus.GetInfo();
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return $"c‚è”­“®‰ñ”F{remain}";
    }
}
