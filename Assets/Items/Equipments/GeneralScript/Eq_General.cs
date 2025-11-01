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

    [SerializeField, Header("\n\nTunrStart")] bool onTS;
    public bool TS_onlyMyTurn;
    [SerializeField] bool onTS_targetSelf;
    [SerializeField] Action.ActionStatus action_TS;
    [SerializeField] CharactersManager.SearchCharaCondition condition_TS;
    [SerializeField] int targetCount_TS;


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

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (TS_onlyMyTurn && !myTurn) return;
        if (onTS)
        {
            if (onTS_targetSelf) { Enqueue_Self(action_TS); }
            else { Enqueue_SearchTarget(action_TS, condition_TS, targetCount_TS); }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += onBS ? action_BS.GetInfo() + "\n" : "";
        s += onRS ? action_RS.GetInfo() + "\n" : "";
        s += onTS ? action_TS.GetInfo() + "\n" : "";
        return s;
    }
}
