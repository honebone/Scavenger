using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_Focused : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddFocused(true);
    }
    //public override void BecomeAbilityTarget(Character actor)
    //{
    //    if ((charaStatus.position < 9) != (actor.GetCharacterStatus().position < 9))
    //    {
    //        AddStack(-1);
    //    }
    //}
    public override void AtTheEnd()
    {
        character.AddFocused(false);
    }
}
