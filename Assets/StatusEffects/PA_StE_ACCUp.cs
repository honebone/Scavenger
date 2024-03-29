using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ACCUp : PA_StatusEffect
{ 
    public override void OnPAInit()
    {
        character.AddACC(StEStatus.stack);
    }
    public override void OnAttack(bool evadeed, bool missed)
    {
        Enqueue_Disable();
    }
    public override void OnAddStack(int add)
    {
        character.AddACC(add);
    }
    public override void AtTheEnd()
    {
        character.AddACC(StEStatus.stack * -1);
    }
}
