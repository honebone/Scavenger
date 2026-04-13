using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_GuidingLight : RE_RandomEvents
{
    [SerializeField] int chance = 50;
    public override void StartRandomEvent()
    {
        players.ForEach(p =>
        {
            if (chance.Dice()) expeditionManager.SetRandomPer_WithType(p, PA_Personality.PersonalityStatus.PersonalityType.good);
        });

        EndRoomEvent();
    }
}
