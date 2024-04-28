using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_CRITCUp : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddCRITC(StEStatus.value);
    }
    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        AddStack(-1);
    }
   
    public override void AtTheEnd()
    {
        character.AddCRITC(StEStatus.value * -1);
    }
}
