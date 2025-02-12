using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Bodybag : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] ActionMod.ActionModStatus actionModStatus;

    [SerializeField] int CDRound;
    int CD;
    bool available;

    //public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    //{
    //    if (statusRef.summon)
    //    {
    //        ActionMod.ActionModStatus mod = actionModStatus;
    //        mod.exINTDMG_int = (character.CharaStatus().maxHP * DMGRatio / 100f).ToInt();
    //        for (int i = 0; i < statusRef.actionTargets.Count; i++)
    //        {
    //            if (statusRef.actionTargets[i].CharaStatus().focused > 0)
    //            {
    //                actionsStatus[i] = actionsStatus[i].Modify(mod);
    //            }
    //        }
    //    }

    //    return actionsStatus;
    //}

    public override void OnBattleStart()
    {
        available = true;
        CD = 0;
    }
    public override void OnRoundStart()
    {
        if (CD > 0)
        {
            CD--;
            if (CD == 0)
            {
                Log("再発動可能");
                available = true;
            }
        }
    }

    public override void OnKill(List<Action.OnKillParams> onKillParamsList)
    {
        if (available)
        {
            foreach (Action.OnKillParams onKillParams in onKillParamsList)
            {
                if (onKillParams.target.CharaStatus().characterTags.Contains(CharacterData.CharacterTag.corpse))
                {
                    infoText.AddDebugText("corpseKilled");
                    if(Enqueue_SearchTarget(actionStatus, condition, 1))
                    {
                        available = false;
                        CD = CDRound;
                    }
                    break;
                }
            }
        }
    }

    public override void OnBattleEnd()
    {
        available = true;
        CD = 0;
    }
    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo();
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return (available) ? "発動可能" : $"再発動まで{CD}ラウンド";
    }
}
