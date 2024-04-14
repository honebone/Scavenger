using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ATKUp : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddATK(0, StEStatus.stack);
    }
    public override void OnAttack(bool evaded, bool missed)
    {
        Disable(); 
    }
    public override void OnAddStack(int add)
    {
        character.AddATK(0, add);
    }
    public override void AtTheEnd()
    {
        character.AddATK(0, StEStatus.stack * -1);
    }
}
