using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_InjuredSpirit : RE_RandomEvents
{
    [SerializeField] int heal;
    [SerializeField] int healReq;
    [SerializeField] int exp;

    [SerializeField] List<REOptionParams> options;

    public override void StartRandomEvent()
    {
        int heals = characterManager.GetPartyAbilityAmount(AbilityData.AbilityType.heal);

        options[0].available = heals >= healReq;

        expeditionManager.SetREOptionButtons(options);
    }

    public override void SelectOption(int index)
    {
        if (index == 0)
        {
            lootPanel.AddExp(exp);
            lootPanel.Loot();
        }
        else if(index==1)
        {
            players.ForEach(p =>
            {
                expeditionManager.SetRandomPer_WithType(p, PA_Personality.PersonalityStatus.PersonalityType.good);
            });

            EndRoomEvent();
        }
        else
        {
            players.ForEach(p =>
            {
                expeditionManager.SetRandomPer_WithType(p, PA_Personality.PersonalityStatus.PersonalityType.bad);
                p.Heal_Per(heal);
            });

            EndRoomEvent();
        }
    }
}
