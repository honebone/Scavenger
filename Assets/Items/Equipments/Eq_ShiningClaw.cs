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
    public override void OnDamage(Action.OnDamageParams onDamageParams)
    {
        if (onDamageParams.actionStatus.abilityEffect)
        {
            character.Enqueue(claw, true, new List<Character> { onDamageParams.target });
            character.Enqueue(claw, true, new List<Character> { onDamageParams.target });
        }
    }
}
