using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PA_Equipment;

public class PA_Per_OnSomeoneFocus : PA_Personality
{
    [Header("trueならダメージを与えた敵1体につき")] public bool foreachTarget;
    public bool excludeSelf;
    public bool onlyAlly;
    public bool onlyOpponent;
    public int max_inBattle;
    public int max_inRound;
    public bool targetSelf;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public int targetCount;

    int count_inBattle;
    int count_inRound;
    bool available;

    public override void OnSomeoneFocus(List<Action.OnFocusParams> focusParamsList)
    {
        if (available)
        {
            foreach (var focus in focusParamsList)
            {
                if (excludeSelf && focus.actionParams.owner == character) continue;
                if(onlyAlly && focus.actionParams.owner.PlayerPos() != character.PlayerPos())continue;
                if(onlyOpponent && focus.actionParams.owner.PlayerPos() == character.PlayerPos())continue;

                Activate();
                if (!foreachTarget) break;
                if (!available) break;
            }
        }
    }

    public override void OnBattleStart()
    {
        ResetCount();
    }

    public override void OnRoundEnd()
    {
        count_inRound = 0;
        SetAvailable();
    }

    public override void OnBattleEnd()
    {
        ResetCount();
    }
    void Activate()
    {
        if (targetSelf)
        {
            Enqueue_Self(actionStatus);
            count_inBattle++;
            count_inRound++;
        }
        else
        {
            if (Enqueue_SearchTarget(actionStatus, condition, targetCount))
            {
                count_inBattle++;
                count_inRound++;
            }
        }
        SetAvailable();
    }
    void ResetCount()
    {
        count_inBattle = 0;
        count_inRound = 0;
        available = true;
    }

    void SetAvailable()
    {
        available = (max_inBattle == 0 || count_inBattle < max_inBattle) && (max_inRound == 0 || count_inRound < max_inRound);
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }
    public override string GetCurrentStateInfo()
    {
        if (max_inBattle > 0) return $"残り発動回数：{max_inBattle - count_inBattle}回";
        if (max_inRound > 0) return $"残り発動回数：{max_inRound - count_inRound}回";
        return "";
    }
}
