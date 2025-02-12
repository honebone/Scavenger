using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Summon : PA_Personality
{
    public override void OnSummoned(Action.OnSummonParams onSummonParams)
    {
        character.AddHide(true);
    }
}
