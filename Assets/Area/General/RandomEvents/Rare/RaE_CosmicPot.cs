using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_CosmicPot : RE_RandomEvents
{
    [SerializeField] float exp;
    [SerializeField] int SANDMG;
    [SerializeField] List<REOptionParams> options;

    int state;

    public override void StartRandomEvent()
    {
        expeditionManager.SetREOptionButtons(options);
    }

    public override void SelectOption(int index)
    {
        if(state == 0)
        {
            state = 1;
            if (index == 0)
            {
                expeditionManager.SetREOptionButtons(GenPlayerSelects());
            }
            else if (index == 1)
            {
                EndRoomEvent();
            }
        }
        else if (state == 1)
        {
            players[index].SANDamage(SANDMG);
            players[index].GainEXP(expeditionManager.GetExpAmount(exp));

            EndRoomEvent();
        }
    }
}
