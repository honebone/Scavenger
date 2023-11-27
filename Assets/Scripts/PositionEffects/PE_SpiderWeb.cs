using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PE_SpiderWeb : PositionEffect
{
    public override void OnPEInit()
    {
        if (character != null)
        {
            ApplyEffect(true);
        }
    }
    public override void OnCharaEnter()
    {
        ApplyEffect(true);
    }

    public override void OnCharaLeave()
    {
        ApplyEffect(false);
    }
    public override void OnRoundEnd()
    {
        AddStack(-1);
    }
    public override void AtTheEnd()
    {
        if(character != null) { ApplyEffect(false); }
    }

    void ApplyEffect(bool set)
    {
        int i = 1;
        if (!set) { i = -1; }
        character.AddACT(-3*i);
        character.AddEVD(-10*i);
    }
}
