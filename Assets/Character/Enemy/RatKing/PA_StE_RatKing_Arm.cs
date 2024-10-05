using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_RatKing_Arm : PA_StatusEffect
{
    [SerializeField] int ATKPerStack;
    [SerializeField] int ACTPerStack;

    public override void OnPAInit()
    {
        character.AddATK(0, StEStatus.stack * ATKPerStack);
        character.AddACT(StEStatus.stack * ACTPerStack);
    }

    public override void OnAddStack(int add)
    {
        character.AddATK(0, add * ATKPerStack);
        character.AddACT(add * ACTPerStack);
    }
  
    public override void AtTheEnd()
    {
        character.AddATK(0, StEStatus.stack * ATKPerStack * -1);
        character.AddACT(StEStatus.stack * ACTPerStack * -1);
    }
}
