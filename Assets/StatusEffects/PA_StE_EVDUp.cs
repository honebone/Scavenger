using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_EVDUp : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddEVD(StEStatus.value);
    }
    public override void OnAttacked(Character attacker, bool evaded, bool missed)
    {
        AddStack(-1);
    }
    public override void OnAddStack(int add)
    {
        character.AddEVD(add);
    }
    public override void AtTheEnd()
    {
        character.AddEVD(StEStatus.value * -1);
    }
}
