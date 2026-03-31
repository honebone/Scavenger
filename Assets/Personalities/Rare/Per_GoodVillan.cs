using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_GoodVillan : PA_Personality
{
    [SerializeField] int exDMG;
    int bad;
    public override void OnBattleStart()
    {
        bad = character.GetPers(PersonalityStatus.PersonalityType.bad).Count;
        if(bad > 0)
        {
            character.AddExDMG_Mul(bad * exDMG);
            Log($"与ダメージ+{bad * exDMG}％");
        }
    }

    public override void OnBattleEnd()
    {
        character.AddExDMG_Mul(-bad * exDMG);
        bad = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"与ダメージ+{bad * exDMG}％";
    }
}
