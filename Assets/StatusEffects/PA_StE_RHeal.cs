using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_RHeal : PA_StatusEffect
{
    [SerializeField] bool up;
    int n;
    public override void OnPAInit()
    {
        n = up ? 1 : -1;
        character.AddRHeal(StEStatus.value * n);
    }

    //public override void OnHealed(Character healer, Action.OnHealParams onHealParams)
    //{
    //    AddStack(-1);
    //}

    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn && applyFlag) { AddStack(-1); }
    }

    public override void AtTheEnd()
    {
        character.AddRHeal(StEStatus.value * -1 * n);
    }
}
