using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_SpiderWeb : PA_StatusEffect
{
    
    public override void OnPAInit()
    {
        character.AddACT(-3);
        character.AddEVD(-10);
    }
    public override void OnActivateAbility()
    {
        Enqueue_AddStack(-1);
    }
    public override void AtTheEnd()
    {
        character.AddACT(3);
        character.AddEVD(10);
    }
}
