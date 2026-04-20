using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnAppliedStE : Per_Master
{
    [Header("true‚È‚ç•t—^‚³‚ê‚½ƒoƒt1‚Â‚É‚Â‚«")] public bool foreachStE;
    [SerializeField] List<PA_StatusEffect.StatusEffectStatus.StatusEffectType> targetType;
    [SerializeField] bool toApplyer;

    public override void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams)
    {
        foreach (var applied in onApplyStEParams.appliedParams)
        {
            if (targetType.Contains(applied.GetStatusEffectStatus().StEType))
            {
                if (toApplyer) Activate(new List<Character> { onApplyStEParams.actionParams.owner });
                else Activate();
                if (!foreachStE) break;
            }
        }
    }
}
