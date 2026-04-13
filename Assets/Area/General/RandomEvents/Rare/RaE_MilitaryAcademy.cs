using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RaE_MilitaryAcademy : RE_RandomEvents
{
    [SerializeField] int badPers;
    [SerializeField] int exp;
    [SerializeField] REOptionParams option_rarePer;
    [SerializeField] REOptionParams option_exp;
    int curriculum;

    GameObject per;
    PassiveAbility PA;
    public override void StartRandomEvent()
    {
        result = SelectCurriculum;

        per = expeditionManager.GetPer_Random_CertainType(PA_Personality.PersonalityStatus.PersonalityType.awoken)[0];
        PA = per.GetComponent<PassiveAbility>();
        REOptionParams rarePer = new REOptionParams(option_rarePer);
        rarePer.optionInfo += $"ÉLÉÉÉâ1ëÃÇ™{PA.GetPAName()}Ç∆{badPers}Ç¬ÇÃ<color=#C900FF>à´Ç¢ì¡ê´</color>ÇìæÇÈ\n{PA.GetPAName()}ÅF\n{PA.GetPAInfo()}";

        expeditionManager.SetREOptionButtons(new List<REOptionParams>() { rarePer, option_exp, option_exit });
    }

    void SelectCurriculum(int index)
    {
        if (index == 0 || index == 1)
        {
            curriculum = index;

            expeditionManager.SetREOptionButtons(GenPlayerSelects());

            result = SelectChara;
        }
        else
        {
            EndRoomEvent();
        }
    }

    void SelectChara(int index)
    {
        if (index < players.Count)
        {
            Character selected = players[index];
            expeditionManager.SetRandomPer_WithType(selected, PA_Personality.PersonalityStatus.PersonalityType.bad, badPers);
            if (curriculum == 0)
            {
                expeditionManager.SetPersonality(selected, per);
            }
            else
            {
                selected.GainEXP(expeditionManager.GetExpAmount(exp));
            }
        }
        EndRoomEvent();
    }
}
