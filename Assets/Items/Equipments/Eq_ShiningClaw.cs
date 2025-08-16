using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_ShiningClaw : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus claw;

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += claw.GetInfo(false, new Character.CharacterStatus());
        s += "Ç±ÇÍÇ2âÒçsÇ§";
        return s;
    }
    public override void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        foreach (Action.OnDamageParams onDamageParams in onDamageParamsList)
        {
            if (onDamageParams.ap.actionStatus.abilityEffect)
            {
                Enqueue(claw, true, new List<Character> { onDamageParams.ap.target });
                Enqueue(claw, true, new List<Character> { onDamageParams.ap.target });
            }
        }
    }
}
