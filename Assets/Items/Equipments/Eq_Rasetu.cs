using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Rasetu : PA_Equipment
{
    [SerializeField]
    int maxStack;
    int stack;
    [SerializeField]
    int valuePerStack;
    int currentValue;

    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        AddValue();
    }

    public override void OnRoundStart()
    {
        ResetValue();
    }

    public override void OnBattleEnd()
    {
        ResetValue();
    }

    void ResetValue()
    {
        character.AddATK(0, currentValue * -1);

        stack = 0;
        currentValue = 0;
    }
    void AddValue()
    {
        character.AddATK(0, currentValue * -1);

        stack = Mathf.Min(stack + 1, maxStack);
        character.AddATK(0, stack * valuePerStack);
        currentValue = stack * valuePerStack;
        Log($"ATK+{currentValue}Åì");
    }

    public override string GetCurrentStateInfo()
    {
        return string.Format("åªç›+{0}Åì", currentValue);
    }
}
