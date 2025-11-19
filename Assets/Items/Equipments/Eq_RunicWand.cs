using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eq_RunicWand : PA_Equipment
{
    public int INTPerRune;
    int INT;
    public override void OnBattleStart()
    {
        INT = 0;
        character.GetRunes().ForEach(x =>
        {
            INT += INTPerRune;
            x.ChargeRune(1);
        });
        Log($"{"INT".ToSpr_withName()}+{INT}Åì");
        character.AddINT(0,INT);
    }

    public override void OnBattleEnd()
    {
        character.AddINT(0, -INT);
    }
}
