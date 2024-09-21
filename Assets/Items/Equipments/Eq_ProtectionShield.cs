using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_ProtectionShield : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override void OnBattleStart()
    {
        Enqueue_Self(actionStatus);
    }
}
