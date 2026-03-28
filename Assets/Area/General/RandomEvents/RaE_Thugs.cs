using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AreaManager;

public class RaE_Thugs : RE_RandomEvents
{
    [SerializeField] ExpeditionManager.BattleParams battleParams;
    [SerializeField] List<EnemySet> enemySet;
    [SerializeField] int fee;
    [SerializeField] List<REOptionParams> options;
    public override void StartRandomEvent()
    {
        options[0].available = inventory.CheckCoin(fee);
        expeditionManager.SetREOptionButtons(options);
    }

    public override void SelectOption(int index)
    {
        if (index == 0)
        {
            inventory.RemoveCoin(fee);
            EndRoomEvent();
        }
        else
        {
            expeditionManager.Battle(enemySet, null, battleParams);
        }
    }
}
