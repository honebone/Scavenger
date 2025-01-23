using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_General : PA_Equipment
{
    [SerializeField, Header("\n\nBattleStart")] bool onBS;
    [SerializeField] bool onBS_targetSelf;
    [SerializeField] Action.ActionStatus action_BS;
    [SerializeField] CharactersManager.SearchCharaCondition condition_BS;
    [SerializeField, Header("0Ç»ÇÁèåèÇñûÇΩÇ∑ëSàıÇëŒè€")] int targetCount_BS;

    [SerializeField,Header("\n\nRoundStart")] bool onRS;
    [SerializeField] bool onRS_targetSelf;
    [SerializeField] Action.ActionStatus action_RS;
    [SerializeField] CharactersManager.SearchCharaCondition condition_RS;
    [SerializeField] int targetCount_RS;


    public override void OnBattleStart()
    {
        if (onBS)
        {
            if (onBS_targetSelf) { Enqueue_Self(action_BS); }
            else { Enqueue_SearchTarget(action_BS, condition_BS, targetCount_BS); }
        }
    }

    public override void OnRoundStart()
    {
        if (onRS)
        {
            if (onRS_targetSelf) { Enqueue_Self(action_RS); }
            else { Enqueue_SearchTarget(action_RS, condition_RS, targetCount_RS); }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += onBS ? action_BS.GetInfo() + "\n" : "";
        s += onRS ? action_RS.GetInfo() + "\n" : "";
        return s;
    }
}
