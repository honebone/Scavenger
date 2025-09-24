using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderData;

public class Eq_CounterMagicCloak : PA_Equipment
{
    public int maxUses;
    int remain;
    public override void OnBattleStart()
    {
        remain = maxUses;
    }

    public override void OnAttacked(Character attacker, bool evaded, bool missed)
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
