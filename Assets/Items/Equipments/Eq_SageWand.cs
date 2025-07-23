using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_SageWand : PA_Equipment
{
    public int casts;
    public override void OnRoundStart()
    {
        foreach(var magic in character.GetMagics().Sample(casts))
        {
            magic.Cast();
        }
    }
}
