using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_MultiCaster : PA_Personality
{
    [SerializeField] int castChance;
    public override void OnCast(PassiveAbility cast)
    {
        if (castChance.Dice())
        {
            Log("マルチキャスト！");
            cast.Cast();
        }
    }
}
