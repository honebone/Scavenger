using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Battle : RoomEvent
{
    public ExpeditionManager.BattleParams battleParams;
    public int waveMod;
    public override void StartRoomEvent()
    {
        List<AreaManager.EnemySet> waves = new List<AreaManager.EnemySet>();    
        for (int i = 0; i < 1+waveMod; i++)
        {
            waves.Add(currentArea.GetRandomEnemySet());
        }
        expeditionManager.Battle(waves, currentArea.GetRandomFE(), battleParams);
    }
    public override void OnEndBattle()
    {
        supplyManager.AddSupply_Eq(partyStatus.supplyOptions);
        lootPanel.Loot();
    }

}
