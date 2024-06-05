using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_Tutorial : RE_RandomEvents
{

    public override void StartRandomEvent()
    {
        StartCoroutine(Consequence());
    }


    IEnumerator Consequence()
    {
        foreach (Character c in characterManager.GetExistingCharacters_All())
        {
            if (c.GetCharacterStatus().playable) { expeditionManager.SetRandomPersonality(c); }
        }

        yield return new WaitForSeconds(1.5f);
        expeditionManager.StartTutorial_Personality();
        EndRoomEvent();
    }
}
