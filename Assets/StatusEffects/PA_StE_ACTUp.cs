using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ACTUp : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddACT(StEStatus.value);
    }
    public override void OnTurnOrderDecide()
    {
        AddStack(-1);
    }

    public override void AtTheEnd()
    {
        character.AddACT(StEStatus.value * -1);
    }
}
