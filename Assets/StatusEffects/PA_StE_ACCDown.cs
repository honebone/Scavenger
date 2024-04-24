using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ACCDown : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddACC(StEStatus.value * -1);
    }
    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        AddStack(-1);
    }
    public override void OnAddStack(int add)
    {
        character.AddACC(add * -1);
    }
    public override void AtTheEnd()
    {
        character.AddACC(StEStatus.value);
    }
}
