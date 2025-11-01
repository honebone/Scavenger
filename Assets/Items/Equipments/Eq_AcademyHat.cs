using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Eq_AcademyHat : PA_Equipment
{
    public int INT;
    public int maxCount;
    public float INT_base_mul;
    int count;
    int INT_base;

    public override void OnCast(PassiveAbility cast)
    {
        if (count < maxCount)
        {
            character.AddINT(0, INT);
            count++;
            Log($"カウント+1({count})");
            if (count == maxCount)
            {
                Log($"カウント最大!!");
                INT_base = (character.CharaStatus().BaseINT() * INT_base_mul / 100f).ToInt();
                character.AddINT(INT_base, 0);
            }
        }
    }

    public override void OnBattleEnd()
    {
        character.AddINT(-INT_base, -INT * count);
        count = 0;
        INT_base = 0;
    }

    public override string GetCurrentStateInfo()
    {
        string s = $"カウント：{count}/{maxCount}({"INT".ToSpr_withName()}+{INT * count}％)";
        if (count == maxCount) s += $"\nカウント最大ボーナス：{"INT".ToSpr_withName()}基礎値+{INT_base}";
        return s;
    }
}
