using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_OnAttacked : Eq_Master
{
    [SerializeField] bool onlyEvaded;
    [SerializeField] bool onlyHit;
    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        if (onlyEvaded && !onAttackParams.evaded) return;
        if (onlyHit && onAttackParams.evaded) return;

        Activate();
    }
}
