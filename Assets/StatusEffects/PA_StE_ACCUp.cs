using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ACCUp : PA_StatusEffect
{ 
    public override void OnPAInit()
    {
        character.AddACC(StEStatus.value);
    }
    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        AddStack(-1);
    }
    public override void OnAddStack(int add)
    {
        character.AddACC(add);
    }
    public override void AtTheEnd()
    {
        character.AddACC(StEStatus.value * -1);
    }
}
