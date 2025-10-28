using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_CounterMagicCloak : PA_Equipment
{
    public int maxUses;
    int remain;
    public override void OnBattleStart()
    {
        remain = maxUses;
    }

    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        if (remain > 0)
        {
            remain--;
            foreach (var magic in character.GetMagics().Sample(1))
            {
                magic.Cast();
            }
        }
    }

    public override void OnRoundEnd()
    {
        remain = maxUses;
    }

    public override void OnBattleEnd()
    {
        remain = maxUses;
    }

    public override string GetCurrentStateInfo()
    {
        return $"”­“®‰Â”\‰ñ”F{remain}/{maxUses}";
    }
}
