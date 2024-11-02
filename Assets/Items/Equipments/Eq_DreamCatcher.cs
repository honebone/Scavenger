using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_DreamCatcher : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    bool act;
    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }

    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        act = true;
    }
    public override void OnTurnEnd()
    {
        if (!act)
        {
            Enqueue_Self(actionStatus);
        }
        act = false;
    }
}
