using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Key : PA_Equipment
{
    [SerializeField] ItemData.Rarity rarity;
    [SerializeField] Vector2Int coin;
    [SerializeField] int breakChance;
    [SerializeField] ItemData broken;
    public override void OnBattleEnd()
    {
        LootPanel.inst.AddCoin(coin.Range());
        LootPanel.inst.AddItem(ExpeditionManager.inst.GetRandomEquipment_WithRarity(rarity));

        if (breakChance.Dice())
        {
            if (broken != null)
            {
                Log("óÚâªÅI");
                LootPanel.inst.AddItem(broken);
            }
            else
            {
                Log("âÛÇÍÇΩÅI");
            }
            character.UnequipItem(this, false);
        }
    }
}
