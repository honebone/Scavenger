using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_CommanderWhistle : PA_Equipment
{
    public int maxUseage;
    public int stackTH;
    public GameObject focus;
    public Action.ActionStatus actionStatus;

    int remain;
    int count;

    public override void OnBattleStart()
    {
        remain = maxUseage;
    }

    public override void OnRoundStart()
    {
        remain = maxUseage;
    }

    public override void OnApplyStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        if (remain > 0)
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
            while (count >= stackTH && remain > 0)
            {
                remain--;
                Enqueue_Self(actionStatus);
                count -= stackTH;
            }
        }

    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"発動可能回数：{remain}/{maxUseage}\nカウント：{count}/{stackTH}";
    }
}
