using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnAttacked : Per_Master
{
    [SerializeField] bool onlyHit;
    [SerializeField] bool onlyEvaded;
    [SerializeField] bool onlyCRIT;
    [SerializeField] bool toAttacker;
    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        if (onlyHit && !onAttackParams.hit) return;
        if (onlyEvaded && !onAttackParams.evaded) return;
        if (onlyCRIT && !onAttackParams.CRIT) return;

        if (toAttacker) Activate(new List<Character> { onAttackParams.actionParams.owner });
        else Activate();
    }
}
