using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ACTDown : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddACT(StEStatus.stack * -1);
    }
    public override void OnTurnOrderDecide()
    {
        Enqueue_Disable();
    }
    public override void OnAddStack(int add)
    {
        character.AddACT(add * -1);
    }
    public override void AtTheEnd()
    {
        character.AddACT(StEStatus.stack);
    }
}
