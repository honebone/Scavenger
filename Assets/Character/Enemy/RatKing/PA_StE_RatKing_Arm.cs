using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_RatKing_Arm : PA_StatusEffect
{
    [SerializeField] int ATKPerStack;

    public override void OnPAInit()
    {
        character.AddATK(0, StEStatus.stack * ATKPerStack);
    }

    public override void OnAddStack(int add)
    {
        character.AddATK(0, add * ATKPerStack);
    }
  
    public override void AtTheEnd()
    {
        character.AddATK(0, StEStatus.stack * ATKPerStack * -1);
    }
}
