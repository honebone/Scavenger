using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_MidasHand : PA_Equipment
{
    [SerializeField] int amount;
    [SerializeField] int priceMul;
    public override void OnBattleEnd()
    {
        Inventory.inst.GetEquipments().Sample(amount).ForEach(eq =>
        {
            Log($"{eq.GetName()}‚ð”j‰ó");
            Inventory.inst.RemoveItem(eq, 1);
            int coin = GameManager.gameParams.eqPrice[(int)eq.data.rarity].Mul(priceMul);
            Log($"{"coin".ToSpr_withName()}‚ð{coin}Šl“¾");
            LootPanel.inst.AddCoin(coin);
        });
    }
}
