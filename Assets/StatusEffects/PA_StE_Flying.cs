using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Flying : PA_StatusEffect
{
    public int value;
    public override void OnPAInit()
    {
        character.AddEVD(value);
    }
    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        AddStack(-1);
    }

    //public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    //{
    //    if (myTurn && applyFlag) { AddStack(-1); }
    //}

    public override void AtTheEnd()
    {
        character.AddEVD(value * -1);
    }
}
