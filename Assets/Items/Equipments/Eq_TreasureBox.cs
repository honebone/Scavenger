using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_TreasureBox : PA_Equipment
{
    [SerializeField] int openChance;
    [SerializeField] LootPanel.LootStatus loot;
    public override void OnBattleEnd()
    {
        if (openChance.Dice())
        {
            Log("äJè˘ê¨å˜ÅI".ColorStr(Definer.colorRef.emphasize));
            LootPanel.inst.DropItem_Loot(loot);
            character.UnequipItem(this, false);
        }
        else
        {
            Log("äJè˘é∏îs".ColorStr(Definer.colorRef.failed_unavailable));
        }
    }
}
