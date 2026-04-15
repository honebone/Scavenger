using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class RaE_Dinner : RE_RandomEvents
{
    [SerializeField] int epicReq;
    [SerializeField] int badChance;
    [SerializeField] int coin;
    [SerializeField] Vector2Int SANDMG;
    [SerializeField] List<REOptionParams> options;
    GameObject PA;

    public override void StartRandomEvent()
    {
        PA = expeditionManager.GetPer_Random_CertainType(PA_Personality.PersonalityStatus.PersonalityType.awoken)[0];

        int epics=0;
        players.ForEach(p =>
        {
            epics += p.CharaStatus().equipments.Select(e => e.data.rarity == ItemData.Rarity.epic || e.data.rarity == ItemData.Rarity.legendary).Count();
        });

        options[0].optionInfo += GenPerOptionInfo(PA, "ƒLƒƒƒ‰1‘Ì‚ª");
        options[0].available = epics >= epicReq;

        result = SelectAction;
        expeditionManager.SetREOptionButtons(options);
    }

    void SelectAction(int index)
    {
        if(index == 0)
        {
            result = SelectPlayer;
            expeditionManager.SetREOptionButtons(GenPlayerSelects());
        }
        else if (index==1)
        {
            lootPanel.AddCoin(coin);
            lootPanel.Loot();
            players.ForEach(p =>
            {
               if(badChance.Dice()) expeditionManager.SetRandomPer_WithType(p,PA_Personality.PersonalityStatus.PersonalityType.bad);
            });
        }
        else if (index == 2)
        {
            players.ForEach(p =>
            {
                p.SANDamage(SANDMG.Range());
                expeditionManager.SetRandomPer_WithType(p, PA_Personality.PersonalityStatus.PersonalityType.good);
            });

            EndRoomEvent();
        }
        else
        {
            EndRoomEvent();
        }
    }

    void SelectPlayer(int index)
    {
        expeditionManager.SetPersonality(players[index], PA);

        EndRoomEvent();
    }
}
