using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Gigantic : PA_StatusEffect
{
    [SerializeField]
    float value;
    public override void OnPAInit()
    {
        character.AddATK(0, value);
        character.AddMaxHP(0,value,true);
    }
    public override void AtTheEnd()
    {
        character.AddATK(0, value*-1);
        character.AddMaxHP(0, value*-1, false);
    }
}
