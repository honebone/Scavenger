using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_IdeologicalBook : RE_RandomEvents
{
    List<RoomEvent.REOptionParams> options;
   
    List<Character> pool = new List<Character>();

    int phase;

    public override void StartRandomEvent()
    {
        pool = new List<Character>();
        foreach (Character c in characterManager.GetExistingCharacters_All())
        {
            if (c.GetCharacterStatus().playable) { pool.Add(c); }
        }

        options = new List<REOptionParams>();
        foreach (Character character in pool)
        {
            Character.CharacterStatus status = character.GetCharacterStatus();
            REOptionParams option = new REOptionParams();
            option.optionName = string.Format("{0}‚É“Ç‚Ü‚¹‚é", status.charaName);
            option.optionInfo = "ƒ‰ƒ“ƒ_ƒ€‚È1-3ŒÂ‚Ì“Á«‚ğ“¾‚é";
            options.Add(option);
        }

        expeditionManager.SetREOptionButtons(options);
    }

    public override void SelectOption(int index)
    {
        choice = index;
        StartCoroutine(Consequence());
    }

    IEnumerator Consequence()
    {
        infoText.AddLogText(string.Format("{0}‚Ìl‚¦•û‚É•Ï‰»‚ª–K‚ê‚½", pool[choice].GetCharacterStatus().charaName));
        infoText.SwitchToLog();
        yield return new WaitForSeconds(1.0f);

        int amount = Random.Range(1, 4);
        for (int i = 0; i < amount; i++)
        {
            expeditionManager.SetRandomPersonality(pool[choice]);
        }

        yield return new WaitForSeconds(0.5f);
        EndRoomEvent();
    }
}
