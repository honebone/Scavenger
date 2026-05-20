using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnApplyStE : Per_Master
{
    [Header("trueなら付与したバフ1つにつき")] public bool foreachStE;
    [Header("trueなら付与した対象1体につき")] public bool foreachTarget;
    [SerializeField] List<PA_StatusEffect.StatusEffectStatus.StatusEffectType> targetType;
    [SerializeField] bool toApplyed;


    public override void OnApplyStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        bool activated = false;

        foreach (var applyParams in onApplyStEParamsList)
        {
            if (activated && !foreachTarget) break;
            foreach (var applied in applyParams.appliedParams)
            {
                if (targetType.Contains(applied.GetStatusEffectStatus().StEType))
                {
                    activated = true;
                    if (toApplyed) Activate(new List<Character> {applyParams.taget });
                    else Activate();
                    if (!foreachStE) break;
                }
            }
        }
    }
}
