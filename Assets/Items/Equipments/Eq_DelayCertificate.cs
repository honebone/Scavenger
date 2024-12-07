using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_DelayCertificate : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;

    bool activated;
    bool act;
    public override void OnBattleStart()
    {
        activated = false;
    }
    public override void OnRoundStart()
    {
        activated = false;
    }
    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        act = true;
    }
    public override void OnTurnEnd()
    {
        if (!act&&!activated)
        {
            activated = true;
            Enqueue_Self(actionStatus);
        }
        act = false;
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
