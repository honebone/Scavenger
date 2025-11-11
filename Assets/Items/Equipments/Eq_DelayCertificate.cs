using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_DelayCertificate : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;

    bool activated;
    public override void OnBattleStart()
    {
        activated = false;
    }
    public override void OnRoundStart()
    {
        activated = false;
    }

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (!tep.actThisTurn&&!activated&&tep.myTurn)
        {
            activated = true;
            Enqueue_Self(actionStatus);
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return activated ? "”\—Í”­“®Ï‚İ" : "”\—Í”­“®‰Â”\";
    }
}
