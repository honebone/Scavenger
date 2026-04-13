using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaE_MeetingRoom : RE_RandomEvents
{
    [SerializeField] Vector2Int heal;
    [SerializeField] Vector2Int SANHeal;
    [SerializeField] List<REOptionParams> options;
    List<Character> charaList;
    public override void StartRandomEvent()
    {
        result = SelectAction;

        options[0].available = players.Any(p => p.GetPers_Rand().Count() > 0);
        expeditionManager.SetREOptionButtons(options);
    }
    void SelectAction(int index)
    {
        if (index == 0)
        {
            result = SelectChara;
            charaList = players.Where(p => p.GetPers_Rand().Count > 0).ToList();
            expeditionManager.SetREOptionButtons(GenPlayerSelects_CertainChara(charaList));
        }
        else if (index == 1)
        {
            players.ForEach(p => p.Heal_Per(heal.Range()));
            EndRoomEvent();
        }
        else if(index == 2)
        {
            players.ForEach(p => p.SANHeal(SANHeal.Range()));
            EndRoomEvent();
        }
        else
        {
            EndRoomEvent();
        }
    }


    List<PA_Personality> perList;
    Character selectedChara;
    void SelectChara(int index)
    {
        if (index >= charaList.Count)
        {
            infoText.AddErrorText($"index({index})>=player count{charaList.Count}");
            EndRoomEvent();
            return;
        }

        selectedChara = charaList[index];
        perList = selectedChara.GetPers_Rand();
        List<REOptionParams> options_per = new List<REOptionParams>();
        perList.ForEach(l =>
        {
            REOptionParams option = new REOptionParams();
            option.optionName = l.GetPAName();
            option.optionInfo += GenPerOptionInfo(l, "–¡•û‘Sˆõ‚ª");
            options_per.Add(option);
        });

        result = SelectPer;

        expeditionManager.SetREOptionButtons(options_per);
    }

    void SelectPer(int index)
    {
        if (index >= perList.Count)
        {
            infoText.AddErrorText($"index({index})>=per count{perList.Count}");
            EndRoomEvent();
            return;
        }

        players.ForEach((p) =>
        {
            if (p != selectedChara)
            {
                expeditionManager.SetPersonality(p, perList[index].GetPrefab());
            }
        });

        EndRoomEvent();
    }
}
