using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Tutorial_Battle : RoomEvent
{
    [SerializeField] AreaManager.EnemySet enemySet;


    public override void StartRoomEvent()
    {
        expeditionManager.Battle(enemySet, null);
    }

    public override void OnEndLoot()
    {
        supplyManager.ResetSupplies();
        EndRoomEvent();
    }
}
