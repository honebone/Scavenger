using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_DebuffRes : PA_StatusEffect
{
    [SerializeField] bool up;
    int n;
    public override void OnPAInit()
    {
        n = up ? 1 : -1;
        character.AddDebuffRes( StEStatus.value * n);
    }
    public override void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams)
    {
        bool f = false;

        foreach (StatusEffectParams StEParams in onApplyStEParams.attemptedParams)
        {
            if (StEParams.GetStatusEffectStatus().StEType == StatusEffectStatus.StatusEffectType.debuff)
            {
                f = true;
                break;
            }
        }
        if (f) AddStack(-1);
    }

    public override void AtTheEnd()
    {
        character.AddDebuffRes(StEStatus.value * -1 * n);
    }
}
