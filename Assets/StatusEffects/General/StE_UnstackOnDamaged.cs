using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StE_UnstackOnDamaged : PA_StatusEffect
{
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        AddStack(-1);
    }
}
