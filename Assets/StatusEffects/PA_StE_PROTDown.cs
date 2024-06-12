using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_PROTDown : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddPROT(StEStatus.value * -1);
    }
    public override void OnAttacked(Character attacker, bool evaded, bool missed)
    {
        AddStack(-1);
    }

    public override void AtTheEnd()
    {
        character.AddPROT(StEStatus.value);
    }
}
