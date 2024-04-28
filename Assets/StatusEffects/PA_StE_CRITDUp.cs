using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_CRITDUp : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddCRITD(StEStatus.value);
    }
    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        AddStack(-1);
    }

    public override void AtTheEnd()
    {
        character.AddCRITD(StEStatus.value * -1);
    }
}
