using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_MonsterEgg : RE_RandomEvents
{
    [SerializeField] int buffReq;
    [SerializeField] int heal;
    [SerializeField] int SANDMG;
    [SerializeField] List<REOptionParams> options;

    GameObject PA;

    public override void StartRandomEvent()
    {
        PA = expeditionManager.GetPer_Random_CertainType(PA_Personality.PersonalityStatus.PersonalityType.awoken)[0];

        int buffs = characterManager.GetPartyAbilityAmount(AbilityData.AbilityType.buff);

        options[0].optionInfo += GenPerOptionInfo(PA,"ƒLƒƒƒ‰1‘Ì‚ª");
        options[0].available = buffs >= buffReq;

        result = Select;
        expeditionManager.SetREOptionButtons(options);
    }

    private void Select(int index)
    {
        if (index == 0)
        {
            result = Select_Cook;
            expeditionManager.SetREOptionButtons(GenPlayerSelects());
        }
        else
        {
            result = Select_Eat;
            expeditionManager.SetREOptionButtons(GenPlayerSelects());
        }
    }

    void Select_Eat(int index)
    {
        players[index].SANDamage(SANDMG);
        players[index].Heal_Per(heal);

        EndRoomEvent();
    }

    void Select_Cook(int index)
    {
        expeditionManager.SetPersonality(players[index], PA);

        EndRoomEvent();
    }
}
