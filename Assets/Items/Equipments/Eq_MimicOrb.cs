using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Eq_MimicOrb : PA_Equipment
{
    public override void OnBattleEnd()
    {
        List<Definer.Item> items = character.CharaStatus().equipments.Where(i => i.data != equipmentStatus.itemData).ToList();
        
        if (items.Count > 0)
        {
            ItemData.Rarity rarity=ItemData.Rarity.common;
            items.ForEach(i =>
            {
                if ((int)i.data.rarity > (int)rarity) rarity = i.data.rarity;
            });

            LootPanel.inst.AddItem(items.Where(i => i.data.rarity == rarity).ToList().Choice(), 1);
            character.UnequipItem(this, false);
        }
    }
}
