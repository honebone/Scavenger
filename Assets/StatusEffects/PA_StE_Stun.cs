using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Stun : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddStun(true);
    }

    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn && applyFlag) { AddStack(-1); }
    }
    public override void AtTheEnd()
    {
        character.AddStun(false);

    }
}
