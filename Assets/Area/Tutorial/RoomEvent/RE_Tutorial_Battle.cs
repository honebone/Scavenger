using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Tutorial_Battle : RoomEvent
{
    [SerializeField] AreaManager.EnemySet enemySet;


    public override void StartRoomEvent()
    {
        expeditionManager.Battle(new List<AreaManager.EnemySet> { enemySet }, null, new ExpeditionManager.BattleParams());
    }

    public override void OnEndLoot()
    {
        supplyManager.ResetSupplies();
        EndRoomEvent();
    }
}
