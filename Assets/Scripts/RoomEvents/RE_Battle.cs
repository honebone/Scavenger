using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Battle : RoomEvent
{
    public override void StartRoomEvent()
    {
        expeditionManager.Battle(currentArea.GetRandomEnemySet(),currentArea.GetRandomFE());
    }
    public override void OnEndBattle()
    {
        FindObjectOfType<LootPanel>().OpenLootPanel();
    }
    public override void OnEndLoot()
    {
        EndRoomEvent();
    }
}
