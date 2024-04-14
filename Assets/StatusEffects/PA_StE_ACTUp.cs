using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ACTUp : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddACT(StEStatus.stack);
    }
    public override void OnTurnOrderDecide()
    {
        Disable();
    }
    public override void OnAddStack(int add)
    {
        character.AddACT(add);
    }
    public override void AtTheEnd()
    {
        character.AddACT(StEStatus.stack * -1);
    }
}
