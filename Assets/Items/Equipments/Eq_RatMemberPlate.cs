using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_RatMemberPlate : PA_Equipment
{
    [SerializeField] int coinMul;

    public override void OnBattleEnd()
    {
        int coin=Inventory.inst.GetCoin().Mul(coinMul);
        Log($"{"coin".ToSpr_withName()}+{coin}");
        LootPanel.inst.AddCoin(coin);
    }
}
