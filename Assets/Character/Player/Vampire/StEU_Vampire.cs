using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StEU_Vampire : PA_StatusEffect
{
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn) Disable();
    }
}
