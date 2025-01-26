using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_CRITDUp : PA_StatusEffect
{
    [SerializeField] bool up;
    int n;

    public override void OnPAInit()
    {
        n = up ? 1 : -1;
        character.AddCRITD(StEStatus.value* n);
    }
    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        AddStack(-1);
    }

    public override void AtTheEnd()
    {
        character.AddCRITD(StEStatus.value * -1* n);
    }
}
