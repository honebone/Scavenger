using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Blader_Combo : PA_StatusEffect
{
    [SerializeField] int CRITCPerStack;
    [SerializeField] float CRITDPerStack;

    public override void OnPAInit()
    {
        character.AddCRITC(StEStatus.stack * CRITCPerStack);
        character.AddCRITD(StEStatus.stack * CRITDPerStack);
    }

    public override void OnAddStack(int add)
    {
        character.AddCRITC(add * CRITCPerStack);
        character.AddCRITD(add * CRITDPerStack);
    }
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (onDamageParams.totalDMG > 0) AddStack(-2);
    }

    public override void AtTheEnd()
    {
        character.AddCRITC(StEStatus.stack * CRITCPerStack * -1);
        character.AddCRITD(StEStatus.stack * CRITDPerStack * -1);
    }
}
