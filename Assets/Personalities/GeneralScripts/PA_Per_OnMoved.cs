using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Per_OnMoved : PA_Personality
{
    [Header("0なら100%")]
    public int chance;
    public bool targetSelf;
    public bool onlyPrimalMove;
    public int max_inBattle;
    public int max_inRound;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public int targetCount;

    int count_inBattle;
    int count_inRound;
    bool available;

    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        if (available && (!onlyPrimalMove || !onMoveParams.secondaryMove))
        {
            Activate();
        }
    }

    void Activate()
    {
        if (chance == 0 || chance.Dice())
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
            available = (max_inBattle == 0 || count_inBattle < max_inBattle) && (max_inRound == 0 || count_inRound < max_inRound);
        }
    }

    public override void OnBattleStart()
    {
        count_inBattle = 0;
        count_inRound = 0;
        available = true;
    }

    public override void OnRoundEnd()
    {
        count_inRound = 0;
        available = (max_inBattle == 0 || count_inBattle < max_inBattle) && (max_inRound == 0 || count_inRound < max_inRound);
    }

    public override void OnBattleEnd()
    {
        count_inBattle = 0;
        count_inRound = 0;
        available = true;
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        if (max_inBattle > 0) return $"残り発動回数：{max_inBattle - count_inBattle}回";
        if (max_inRound > 0) return $"残り発動回数：{max_inRound - count_inRound}回";
        return "";
    }
}
