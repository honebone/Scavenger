using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_ShiningClaw : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus claw;

    public override string GetPAInfo()
    {
        string s = equipmentStatus.GetInfo()+"\n\n";
        s += claw.GetInfo(false, new Character.CharacterStatus());
        s += "Ç±ÇÍÇ2âÒçsÇ§";
        return s;
    }
    public override void OnDamage(int DMG, Character target, Action.ActionStatus actionStatus)
    {
        if (actionStatus.abilityEffect && actionStatus.attackType == 0)
        {
            character.Enqueue(claw, true, new List<Character> { target });
            character.Enqueue(claw, true, new List<Character> { target });
        }
    }
}
