using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Bodybag : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] ActionMod.ActionModStatus actionModStatus;

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
    }
    public override void OnRoundStart()
    {
        if (!available)
        {
            Log("çƒî≠ìÆâ¬î\");
            available = true;
        }
    }

    public override void OnSomeoneDied(Character died)
    {
        if (available)
        {
            if (died.CharaStatus().characterTags.Contains(CharacterData.CharacterTag.corpse))
            {
                if (Enqueue_SearchTarget(actionStatus, condition, 1))
                {
                    available = false;
                }
            }
        }
    }

    public override void OnBattleEnd()
    {
        available = true;
    }
    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo();
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return (available) ? "î≠ìÆâ¬î\" : $"î≠ìÆïsâ¬";
    }
}
