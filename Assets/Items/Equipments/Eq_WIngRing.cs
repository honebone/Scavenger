using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_WIngRing : PA_Equipment
{
    public int PROT;
    bool active;
    public override void OnRoundStart()
    {
        if (!active)
        {
            active = true;
            character.AddPROT(PROT);
            Log($"{"PROT".ToSpr_withName()}{PROT.Evaluate()}");
        }
    }

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn && active)
        {
            active = false;
            character.AddPROT(-PROT);
            Log($"{"PROT".ToSpr_withName()}å∏è≠å¯â è¡ãé");
        }
    }

    public override void OnBattleEnd()
    {
        if (active)
        {
            active = false;
            character.AddPROT(-PROT);
            Log($"{"PROT".ToSpr_withName()}å∏è≠å¯â è¡ãé");
        }
    }
}
