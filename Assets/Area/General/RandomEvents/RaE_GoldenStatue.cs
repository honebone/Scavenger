using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_GoldenStatue : RE_RandomEvents
{
    [SerializeField] List<REOptionParams> options;

    public override void StartRandomEvent()
    {
        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        lootPanel.AddCoin(inventory.GetCoin());
        lootPanel.Loot();
    }
}
