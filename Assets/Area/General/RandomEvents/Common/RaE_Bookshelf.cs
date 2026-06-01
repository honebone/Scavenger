using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaE_Bookshelf : RE_RandomEvents
{
    [SerializeField] List<string> bookNames;

    List<GameObject> persList;
    GameObject per;
    public override void StartRandomEvent()
    {
        persList = new List<GameObject>();
        List<REOptionParams> options = new List<REOptionParams>();

        for (int i = 0; i < bookNames.Count; i++)
        {
            GameObject PA= expeditionManager.GetPer_Random_CertainType(PA_Personality.PersonalityStatus.PersonalityType.good)[0];
            persList.Add(PA);

            REOptionParams option = new REOptionParams();
            option.optionName = $"{bookNames[i]}‚ğ“Ç‚Ş";
            option.optionInfo = GenPerOptionInfo(PA, "ƒLƒƒƒ‰1‘Ì‚ª", "‚Æƒ‰ƒ“ƒ_ƒ€‚È<color=#C900FF>ˆ«‚¢“Á«</color>‚ğ1‚Â“¾‚é");
            options.Add(option);
        }

        options.Add(option_exit);
        result = SelectBook;
        expeditionManager.SetREOptionButtons(options);
    }

    void SelectBook(int index)
    {
        if (index < persList.Count)
        {
            per = persList[index];

            result = SelectChara;
            expeditionManager.SetREOptionButtons(GenPlayerSelects());
        }
        else
        {
            EndRoomEvent();
        }
    }

    void SelectChara(int index)
    {
        if (index >= players.Count)
        {
            infoText.AddErrorText($"index({index})>=player count{players.Count}");
            EndRoomEvent();
            return;
        }

        expeditionManager.SetPersonality(players[index], per);
        expeditionManager.SetRandomPer_WithType(players[index], PA_Personality.PersonalityStatus.PersonalityType.bad);
        EndRoomEvent();
    }
}
