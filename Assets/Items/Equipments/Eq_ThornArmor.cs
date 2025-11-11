using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_ThornArmor : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (onDamageParams.ap.owner != null)
        {
            Enqueue(actionStatus, true, new List<Character>() { onDamageParams.ap.owner });
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
}
