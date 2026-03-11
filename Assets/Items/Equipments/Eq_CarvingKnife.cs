using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_CarvingKnife : PA_Equipment
{
    public override void OnBattleEnd()
    {
        SupplyManager.inst.AddSupply_Eq(1);
    }
}
