using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_ChaosSpring : RE_RandomEvents
{
    [SerializeField] Vector2Int coinMul;
    [SerializeField] List<REOptionParams> options;

    int phase = 0;
    public override void StartRandomEvent()
    {
        options.Add(option_exit);
        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        if (phase == 0)
        {
            if (index == 0)
            {
                phase = 1;
                expeditionManager.SetREOptionButtons(GenPlayerSelects());
            }
            else
            {
                lootPanel.AddCoin(inventory.GetCoin().Mul(coinMul.Range()));
                inventory.RemoveCoin(inventory.GetCoin());
                lootPanel.Loot();
            }
        }
        else
        {
            Character selected = players[index];
            List<PA_Personality> list = selected.GetPers_Rand();
            int count = list.Count;
            foreach(PA_Personality p in list)
            {
                p.Disable();
            }
            expeditionManager.SetRandomPer(selected, count);
            EndRoomEvent();
        }
    }
}
