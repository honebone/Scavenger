using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaE_TravelingPriest : RE_RandomEvents
{
    [SerializeField] int price;
    public override void StartRandomEvent()
    {
        List<REOptionParams> list = GenPlayerSelects();
        list.ForEach(l=>l.available=Inventory.inst.CheckCoin(price));
        list.Add(option_exit);
        expeditionManager.SetREOptionButtons(list);
    }

    public override void SelectOption(int index)
    {
        if (index < players.Count)
        {
            Character selected = players[index];
            Inventory.inst.RemoveCoin(price);
            expeditionManager.SetRandomPer_WithType(selected,PA_Personality.PersonalityStatus.PersonalityType.good);
        }

        EndRoomEvent();
    }
}
