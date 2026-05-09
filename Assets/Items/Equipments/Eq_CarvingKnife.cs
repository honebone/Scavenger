using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_CarvingKnife : PA_Equipment
{
    [SerializeField] int chance;
    public override void OnBattleEnd()
    {
        if(chance.Dice()) SupplyManager.inst.AddSupply_Eq(1);
    }
}
