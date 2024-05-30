using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Piggy : PA_Equipment
{
    float currentValue;

    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        bool CRIT = false;
        float value = 0;
        foreach(Action.OnAttackParams p in onAttackParamsList)
        {
            if (!p.missed && !p.evaded)
            {
                if (p.CRIT)
                {
                    CRIT = true;
                    break;
                }
                value += Mathf.Max(0, p.toralCRITC);
            }
        }
        if (CRIT) { ResetValue(); }
        else { AddValue(value); }
    }

    public override void OnBattleEnd()
    {
        ResetValue();
    }

    void ResetValue()
    {
        character.AddCRITC(currentValue * -1);

        currentValue = 0;
    }
    void AddValue(float value)
    {
        character.AddCRITC(currentValue * -1);

        character.AddCRITC(value);
        currentValue = value;
    }

    public override string GetCurrentStateInfo()
    {
        return string.Format("åªç›+{0}Åì", currentValue);
    }
}
