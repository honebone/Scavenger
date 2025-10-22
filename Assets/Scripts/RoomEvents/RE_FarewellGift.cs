using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_FarewellGift : RoomEvent
{
    public int expOrbs;
    public int rareEqs;
    public int eqs;
    public override void OnEndREInfo()
    {
        lootPanel.AddExp(expOrbs);
        for (int i = 0; i < eqs; i++) { lootPanel.AddItem(expeditionManager.GetRandomEquipment(),1); } ;
        for (int i = 0; i < rareEqs; i++) { lootPanel.AddItem(expeditionManager.GetRandomEquipment_WithGuarantee(ItemData.Rarity.rare),1); } ;
        supplyManager.AddSupply_Eq(partyStatus.supplyOptions, ItemData.Rarity.epic);
        //supplyManager.SetSupply_Eq(partyStatus.supplyOptions + 2);
        lootPanel.Loot();
    }
}
