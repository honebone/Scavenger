using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_RatKing_Guts : PA_StatusEffect
{
    [SerializeField] int RHealPerStack;

    public override void OnPAInit()
    {
        character.AddRHeal(StEStatus.stack * RHealPerStack);
    }

    public override void OnAddStack(int add)
    {
        character.AddRHeal(add * RHealPerStack);
    }

    public override void AtTheEnd()
    {
        character.AddRHeal(StEStatus.stack * RHealPerStack * -1);
    }
}
