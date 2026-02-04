using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_PandoraBox : PA_Equipment
{
    public override void OnBattleEnd()
    {
        Inventory.inst.GetEquipments().ForEach(eq =>
        {
            Inventory.inst.RemoveItem(eq, 1);
            LootPanel.inst.AddItem(ExpeditionManager.inst.GetRandomEquipment_WithRarity(eq.data.rarity), 1);
        });
    }
}
