using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_BrawlerBuckle : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    public override void OnAttacked(Action.OnAttackParams onAttackParams)
    {
        Enqueue(actionStatus, true, new List<Character>() { onAttackParams.actionParams.owner });
    }

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += equipmentStatus.GetInfo();
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
