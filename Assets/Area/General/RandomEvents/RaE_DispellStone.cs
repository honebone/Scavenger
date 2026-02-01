using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_DispellStone : RE_RandomEvents
{
    public override void StartRandomEvent()
    {
        expeditionManager.SetREOptionButtons(GenPlayerSelects());
    }

    public override void SelectOption(int index)
    {
        Character selected = players[index];
        selected.RemovePer_Random(4, PA_Personality.PersonalityStatus.PersonalityType.bad);

        EndRoomEvent();
    }
}
