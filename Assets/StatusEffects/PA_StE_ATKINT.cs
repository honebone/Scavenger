using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ATKINT : PA_StatusEffect
{
    [SerializeField] bool up;
    int n;
    public override void OnPAInit()
    {
        n = up ? 1 : -1;
        character.AddATKINT(0, StEStatus.value * n);
    }
    //public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    //{
    //    AddStack(-1);
    //}

    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn && applyFlag) { AddStack(-1); }
    }

    public override void AtTheEnd()
    {
        character.AddATKINT(0, StEStatus.value * -1 * n);
    }
}
