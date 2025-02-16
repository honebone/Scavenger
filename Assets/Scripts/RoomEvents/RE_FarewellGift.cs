using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_FarewellGift : RoomEvent
{
    public override void OnEndREInfo()
    {
        supplyManager.SetSupply_Eq(partyStatus.supplyOptions, ItemData.Rarity.epic);
        //supplyManager.SetSupply_Eq(partyStatus.supplyOptions + 2);
        supplyManager.StartSupply();
    }
}
