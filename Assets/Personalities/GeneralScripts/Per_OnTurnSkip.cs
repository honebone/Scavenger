using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnTurnSkip : Per_Master
{
    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn && !tep.actThisTurn)
        {
            Activate();
        }
    }
}
