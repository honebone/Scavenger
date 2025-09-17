using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_StarBeliever : PA_Personality
{
    public GameObject stardust;
    public Action.ActionStatus onBS;
    public Action.ActionStatus onTE;

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        Enqueue_Self(onBS);
    }
    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn && character.GetStEStack_Sum(stardust) > 0) { Enqueue_Self(onTE); }
    }

    public override string GetPAInfo_Base()
    {
        string s = onBS.GetInfo() + "\n";
        s += onTE.GetInfo();
        return s;
    }
}
