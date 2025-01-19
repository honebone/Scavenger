using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Battle : RoomEvent
{
    public override void StartRoomEvent()
    {
        expeditionManager.Battle(currentArea.GetRandomEnemySet(),currentArea.GetRandomFE(), new ExpeditionManager.BattleParams());
    }
    //public override void OnEndBattle()
    //{
    //    lootPanel.Loot();
    //}
 
}
