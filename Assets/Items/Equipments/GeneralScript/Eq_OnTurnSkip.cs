using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_OnTurnSkip : Eq_Master
{
    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn&&!tep.actThisTurn)
        {
            Activate();
        }
    }
}
