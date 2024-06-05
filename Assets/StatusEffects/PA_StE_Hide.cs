using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Hide : PA_StatusEffect
{
    bool appliedThisTurn = true;

    public override void OnPAInit()
    {
        character.AddHide(true);
    }
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        appliedThisTurn = false;
    }

    public override void OnTurnEnd()
    {
        if (!appliedThisTurn) { AddStack(-1); }
    }
    public override void AtTheEnd()
    {
        character.AddHide(false);

    }
}
