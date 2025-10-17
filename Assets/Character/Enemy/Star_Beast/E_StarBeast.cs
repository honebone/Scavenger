using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_StarBeast : PA_Personality
{
    public Action.ActionStatus actionStatus;
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn) { Enqueue_Self(actionStatus); }
    }
    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo() + "\n";
        return s;
    }
}
