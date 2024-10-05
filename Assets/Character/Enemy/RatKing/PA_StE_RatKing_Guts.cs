using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_RatKing_Guts : PA_StatusEffect
{
    [SerializeField] int RHealPerStack;
    [SerializeField] int debuffResPerStack;

    public override void OnPAInit()
    {
        character.AddRHeal(StEStatus.stack * RHealPerStack);
        character.AddDebuffRes(StEStatus.stack * debuffResPerStack);
    }

    public override void OnAddStack(int add)
    {
        character.AddRHeal(add * RHealPerStack);
        character.AddDebuffRes(add * debuffResPerStack);
    }

    public override void AtTheEnd()
    {
        character.AddRHeal(StEStatus.stack * RHealPerStack * -1);
        character.AddDebuffRes(StEStatus.stack * debuffResPerStack * -1);
    }
}
