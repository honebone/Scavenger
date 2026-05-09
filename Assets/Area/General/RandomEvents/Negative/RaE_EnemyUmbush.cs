using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_EnemyUmbush : RE_RandomEvents
{
    [SerializeField] GameObject FE;
    public override void OnEndREInfo()
    {
        ExpeditionManager.inst.Battle(new List<AreaManager.EnemySet>() { expeditionManager.GetNormalBattleEnemySet() }, FE);
    }
}
