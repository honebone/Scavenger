using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RaE_MilitaryAcademy : RE_RandomEvents
{
    [SerializeField] int price;
    [SerializeField] int badPers;
    [SerializeField] REOptionParams option_rarePer;
    [SerializeField] REOptionParams option_exp;
    [SerializeField] REOptionParams course_normal;
    [SerializeField] REOptionParams course_speed;
    int phase;
    int curriculum;

    GameObject per;
    PassiveAbility PA;
     public override void StartRandomEvent()
    {
        result = SelectCurriculum;

        per = expeditionManager.GetPer_Random_CertainType(PA_Personality.PersonalityStatus.PersonalityType.awoken)[0];
        PA = per.GetComponent<PassiveAbility>();
        REOptionParams rarePer = new REOptionParams(option_rarePer);
        rarePer.optionInfo += $"ÉLÉÉÉâ1ëÃÇ™{PA.GetPAName()}ÇìæÇÈ\n{PA.GetPAName()}ÅF{PA.GetPAInfo()}";

        expeditionManager.SetREOptionButtons(new List<REOptionParams>() { rarePer, option_exp, option_exit });
    }

    delegate void Result(int index);
    Result result;

    public override void SelectOption(int index)
    {
        result(index);
    }

    void SelectCurriculum(int index)
    {
        if (index == 0||index==1)
        {
            curriculum = index;

            REOptionParams normal = new REOptionParams(course_normal);
            normal.available = inventory.CheckCoin(price);
            expeditionManager.SetREOptionButtons(new List<REOptionParams>() { normal, course_speed});

            //result=...
        }
        else
        {
            EndRoomEvent();
        }
    }

    //void SelectCourse(int index)
    //{
    //    if (index == 0)
    //    {
    //        inventory.RemoveCoin(price);
    //    }
    //    else
    //    {
    //        EndRoomEvent();
    //    }
    //}
}
