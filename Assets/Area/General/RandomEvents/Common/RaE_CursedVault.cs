using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_CursedVault : RE_RandomEvents
{
    [SerializeField] Vector2Int coin;
    [SerializeField] REOptionParams option;
    public override void StartRandomEvent()
    {
        expeditionManager.SetREOptionButtons(new List<REOptionParams>() { option,option_exit});
    }

    public override void SelectOption(int index)
    {
        if (index == 0)
        {
            lootPanel.AddCoin(coin.Range());
            lootPanel.Loot();

            expeditionManager.SetRandomPer_ToRandom_WithType(PA_Personality.PersonalityStatus.PersonalityType.bad);
        }
        else
        {
            EndRoomEvent();
        }
    }
}
