using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Action;

public class Eq_OnDamage : PA_Equipment
{
    public bool targetSelf;
    [Header("trueならダメージを与えた敵1体につき")] public bool foreachTarget;
    public bool onlyAbility;
    public bool onlyNonZeroDMG;
    public int max_inBattle;
    public int max_inRound;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;
    public int targetCount;

    int count_inBattle;
    int count_inRound;
    bool available;

    public override void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        if (available && (!onlyAbility || onDamageParamsList[0].ap.actionStatus.abilityEffect))
        {
            foreach (var damage in onDamageParamsList)
            {
                if (damage.totalDMG > 0 || !onlyNonZeroDMG)
                {
                    Activate();
                    if (!foreachTarget) break;
                    if (!available) break;
                }
            }
        }
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
        available = (max_inBattle == 0 || count_inBattle < max_inBattle) && (max_inRound == 0 || count_inRound < max_inRound);
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
