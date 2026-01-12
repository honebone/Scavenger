using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;

public class Eq_ShiningClaw : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus claw;

    public override string GetPAInfo_Base()
    {
        string s = claw.GetInfo();
        s += "Ç±ÇÍÇ2âÒçsÇ§";
        return s;
    }
    public override void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        List<Character> targets = new List<Character>();
        foreach (Action.OnDamageParams onDamageParams in onDamageParamsList)
        {
            if (onDamageParams.ap.actionStatus.abilityEffect)
            {
                targets.Add(onDamageParams.ap.target);
            }
        }
        if(targets.Count > 0)
        {
            Enqueue(claw, true, targets);
            Enqueue(claw, true, targets);
        }
    }
}
