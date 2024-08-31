using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_UnstableBlocks : PA_Equipment
{
    [SerializeField]
    int maxStack;
    int stack;
    [SerializeField]
    int valuePerStack;
    int currentValue;
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (onDamageParams.totalDMG > 0)
        {
            character.AddATK(0, currentValue * -1);

            stack = 0;
            currentValue = 0;
        }
    }

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        character.AddATK(0, currentValue * -1);

        stack = Mathf.Min(stack + 1, maxStack);
        character.AddATK(0, stack * valuePerStack);
        currentValue = stack * valuePerStack;
    }

    public override void OnBattleEnd()
    {
        character.AddATK(0, currentValue * -1);

        stack = 0;
        currentValue = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return string.Format("åªç›+{0}Åì",currentValue);
    }
}
