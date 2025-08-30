using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_CommanderWhistle : PA_Equipment
{
    public int stackTH;
    public GameObject focus;
    public Action.ActionStatus actionStatus;

    int count;

    public override void OnApplyStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        string focusName = focus.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName;
        int stack = 0;
        foreach (var paramsList in onApplyStEParamsList)
        {
            foreach (var list in paramsList.appliedParams)
            {
                if (list.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == focusName)
                {
                    stack += list.stack;
                }
            }
        }
        count += stack;
        if (stack > 0) Log($"カウント+{stack} ({count})");
        while (count >= stackTH)
        {
            Enqueue_Self(actionStatus);
            count -= stackTH;
        }
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}/{stackTH}";
    }
}
