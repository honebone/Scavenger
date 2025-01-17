using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Stun : PA_StatusEffect
{
    bool appliedThisTurn = true;
    public override void OnPAInit()
    {
        character.AddStun(true);
    }
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        appliedThisTurn = false;
    }
    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (!appliedThisTurn&&myTurn) { AddStack(-1); }
    }
    public override void AtTheEnd()
    {
        character.AddStun(false);

    }
}
