using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ATKUp : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddATK(0, StEStatus.value);
    }
    public override void OnAttack(bool evaded, bool missed)
    {
        AddStack(-1); 
    }
    public override void OnAddStack(int add)
    {
        character.AddATK(0, add);
    }
    public override void AtTheEnd()
    {
        character.AddATK(0, StEStatus.value * -1);
    }
}
