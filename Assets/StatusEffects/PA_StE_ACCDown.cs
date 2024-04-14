using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ACCDown : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddACC(StEStatus.stack * -1);
    }
    public override void OnAttack(bool evadeed, bool missed)
    {
        Disable();
    }
    public override void OnAddStack(int add)
    {
        character.AddACC(add * -1);
    }
    public override void AtTheEnd()
    {
        character.AddACC(StEStatus.stack);
    }
}
