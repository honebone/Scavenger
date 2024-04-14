using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Marked : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddMarked(true);
    }
    public override void BecomeAbilityTarget(Character actor)
    {
        if(actor!=null&&(character.GetCharacterStatus().position < 9) != (actor.GetCharacterStatus().position < 9))
        {
           AddStack(-1);
        }
    }
    public override void AtTheEnd()
    {
        character.AddMarked(false);
    }
}
