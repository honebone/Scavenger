using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_RuneGraver : PA_Equipment
{
    [SerializeField] int chance;
    public override void OnRuneActivate(PassiveAbility rune)
    {
        if (chance.Dice()) rune.ChargeRune(1);
    }
}
