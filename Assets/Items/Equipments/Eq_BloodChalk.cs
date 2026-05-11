using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_BloodChalk : PA_Equipment
{
    [SerializeField] int countReq;
    int count = 0;
    public override void OnSomeoneDied(Character died)
    {
        if (!died.IsObstacle())
        {
            count++;
            Log($"ƒJƒEƒ“ƒg+1 ({count})");
            if (count == countReq)
            {
                character.GetMagics().ForEach(m => m.Cast());
                count = 0;
            }
        } 
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }
}
