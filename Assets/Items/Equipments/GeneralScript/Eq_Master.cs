using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Master : PA_Equipment
{
    [SerializeField] int max_inBattle;
    [SerializeField] int max_inRound;
    [SerializeField,Header("能力発動までに必要なアクティブ回数")] int countReq=1;
    [SerializeField] bool targetSelf;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] int targetCount;
    [SerializeField, Header("\n能力発動に必要な[条件を満たすキャラ]の必要人数")] int activeCond_req;
    [SerializeField, Header("能力発動に必要なキャラの条件")] CharactersManager.SearchCharaCondition activeCond;
    [SerializeField] Vector2Int coin;
    [SerializeField] bool modifyStatus;
    [SerializeField] Character.CharaStatusMod statusMod;

    int count;
    int count_inBattle;
    int count_inRound;
    bool available;

    int activateCount;

    protected void Activate(List<Character> targets = null)
    {
        if (targets != null && (CheckAltMode() || targetSelf)) { infoText.AddWarningText("Eq_Master error"); }

        if (available && (activeCond_req == 0 || charactersManager.SearchCharaWithCondition(activeCond, character).Count >= activeCond_req))
        {
            count++;
            if (countReq > 1) Log($"カウント+1 ({count})");
            if (count >= countReq)
            {
                if (CheckAltMode())
                {
                    if (coin != Vector2Int.zero)
                    {
                        int c = coin.Range();
                        LootPanel.inst.AddCoin(c);
                        Log($"{"coin".ToSpr_withName()}+{c}");
                    }
                    if (modifyStatus)
                    {
                        character.ModifyStatus(statusMod, true);
                        Log(statusMod.GetInfo());
                    }
                    count_inBattle++;
                    count_inRound++;
                }
                else
                {
                    if (targetSelf)
                    {
                        Enqueue_Self(actionStatus);
                        count_inBattle++;
                        count_inRound++;
                    }
                    else
                    {
                        if(targets != null)
                        {
                            if (Enqueue(actionStatus, true, targets))
                            {
                                count_inBattle++;
                                count_inRound++;
                            }
                        }
                        else if (Enqueue_SearchTarget(actionStatus, condition, targetCount))
                        {
                            count_inBattle++;
                            count_inRound++;
                        }
                    }
                }
                activateCount++;
                count -= countReq;
                available = (max_inBattle == 0 || count_inBattle < max_inBattle) && (max_inRound == 0 || count_inRound < max_inRound);
            }
        }
    }

    public override void OnBattleStart()
    {
        ResetParams();
    }

    public override void OnRoundEnd()
    {
        count_inRound = 0;
        available = (max_inBattle == 0 || count_inBattle < max_inBattle) && (max_inRound == 0 || count_inRound < max_inRound);
    }

    public override void OnBattleEnd()
    {
        if (modifyStatus && activateCount > 0) character.ModifyStatus(statusMod * activateCount, false);
        ResetParams();
    }

    protected void ResetParams()
    {
        count = 0;
        activateCount = 0;
        count_inBattle = 0;
        count_inRound = 0;
        available = true;
    }

    bool CheckAltMode()
    {
        return coin != Vector2Int.zero || modifyStatus;
    }

    public override string GetPAInfo_Base()
    {
        string s = "";
        if (modifyStatus) s = statusMod.GetInfo();
        else s = actionStatus.GetInfo();

        return s;
    }
    public override string GetCurrentStateInfo()
    {
        string s = "";
        if (countReq > 1) s += $"{Extentions.NL(s)}カウント：{count}/{countReq}";
        if (modifyStatus && activateCount > 0) s += $"{Extentions.NL(s)}{(statusMod * activateCount).GetInfo()}";
        if (max_inBattle > 0) s += $"{Extentions.NL(s)}残り発動回数：{max_inBattle - count_inBattle}回";
        if (max_inRound > 0) s += $"{Extentions.NL(s)}残り発動回数：{max_inRound - count_inRound}回";
        return s;
    }
}
