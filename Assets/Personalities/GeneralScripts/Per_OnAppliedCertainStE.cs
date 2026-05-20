using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Per_OnAppliedCertainStE : Per_Master
{
    [SerializeField] bool foreachStack;
    [SerializeField] GameObject targetStE;
    [SerializeField] bool toApplyer;

    public override void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams)
    {
        foreach (var applied in onApplyStEParams.appliedParams)
        {
            if (targetStE==applied.applyStE)
            {
                for(int i = 0; i < applied.stack; i++)
                {
                    if (toApplyer) Activate(new List<Character> { onApplyStEParams.actionParams.owner });
                    else Activate();
                    if (!foreachStack) return;
                }
            }
        }
    }
}
