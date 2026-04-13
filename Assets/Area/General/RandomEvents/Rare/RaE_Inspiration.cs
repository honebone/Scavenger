using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_Inspiration : RE_RandomEvents
{
    public override void StartRandomEvent()
    {
        expeditionManager.SetRandomPer_WithType(players.Choice(), PA_Personality.PersonalityStatus.PersonalityType.awoken);

        EndRoomEvent();
    }
}
