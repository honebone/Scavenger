using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Loot : RoomEvent
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;

    [SerializeField]
    LootPanel.LootStatus lootStatus;

   
    public override void OnEndREInfo()
    {
        lootPanel = FindObjectOfType<LootPanel>();

        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        lootPanel.DropItem_Loot(lootStatus);
        lootPanel.Loot();
    }
    public override void OnEndLoot()
    {
        EndRoomEvent();
    }
}
