using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_LegendaryStone : RE_RandomEvents
{
    [SerializeField] ItemData stone;
    [SerializeField] int price;
    [SerializeField] REOptionParams option;
    public override void StartRandomEvent()
    {
        REOptionParams o = new REOptionParams(option);
        o.available = Inventory.inst.GetCoin() >= price;
        expeditionManager.SetREOptionButtons(new List<REOptionParams>() { o, option_exit });
    }

    public override void SelectOption(int index)
    {
        if (index == 0)
        {
            Inventory.inst.RemoveCoin(price);
            lootPanel.AddItem(stone);
            lootPanel.Loot();
        }
        else
        {
            EndRoomEvent();
        }
    }
}
