using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_PROTDown : PA_StatusEffect
{

    [SerializeField] bool up;
    int n;
    public override void OnPAInit()
    {
        n = up ? 1 : -1;
        character.AddPROT(StEStatus.value * n);
    }
    //public override void OnAttacked(Character attacker, bool evaded, bool missed)
    //{
    //    AddStack(-1);
    //}

    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn && applyFlag) { AddStack(-1); }
    }

    public override void AtTheEnd()
    {
        character.AddPROT(StEStatus.value * n * -1);
    }

}
