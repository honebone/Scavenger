using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_OnAppliedBuff : Eq_Master
{
    [Header("true‚È‚ç•t—^‚³‚ê‚½ƒoƒt1‚Â‚É‚Â‚«")] public bool foreachBuff;

    public override void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams)
    {
        foreach (var applied in onApplyStEParams.appliedParams)
        {
            if (applied.GetStatusEffectStatus().StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff)
            {
                Activate();
                if (!foreachBuff) break;
            }
        }
    }
}
