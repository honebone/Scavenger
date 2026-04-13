using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_HealingSpring : RE_RandomEvents
{
    [SerializeField] List<REOptionParams> options;

    public override void StartRandomEvent()
    {
        expeditionManager.SetREOptionButtons(options);
    }

    public override void SelectOption(int index)
    {
        players.ForEach(p => {
            p.Heal(p.CharaStatus().maxHP, null);
            p.SANHeal(p.CharaStatus().maxSAN);
        });
        EndRoomEvent();
    }
}
