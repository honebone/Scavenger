using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_RatKing_Head : PA_StatusEffect
{
    [SerializeField] int ACCPerStack;
    [SerializeField] int CRITCPerStack;

    public override void OnPAInit()
    {
        character.AddACC(StEStatus.stack * ACCPerStack);
        character.AddCRITC(StEStatus.stack * CRITCPerStack);
    }

    public override void OnAddStack(int add)
    {
        character.AddACC(add * ACCPerStack);
        character.AddCRITC(add * CRITCPerStack);
    }

    public override void AtTheEnd()
    {
        character.AddACC(StEStatus.stack * ACCPerStack * -1);
        character.AddCRITC(StEStatus.stack * CRITCPerStack * -1);
    }
}
