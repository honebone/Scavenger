using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ATKDown : PA_StatusEffect
{ 
    public override void OnPAInit()
    {
        character.AddATK(0, StEStatus.value * -1);
    }
    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        AddStack(-1);
    }
    public override void OnAddStack(int add)
    {
        character.AddATK(0, add * -1);
    }
    public override void AtTheEnd()
    {
        character.AddATK(0, StEStatus.value);
    }
}
