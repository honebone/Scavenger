using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FE_MeaingfulBattle : FieldEffect
{
    public override string GetFEInfo()
    {
        return "戦闘衆力時、追加で経験のオーブを2個入手できる";
    }
    public override void OnBattleStart()
    {
        FindObjectOfType<LootPanel>().AddExp(1);
    }
}
