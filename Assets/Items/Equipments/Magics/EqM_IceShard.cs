using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_IceShard : Eq_Magic
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] int maxRemain = 5;

    [SerializeField] int maxTargetCount;
    [SerializeField] int CRITDPerTargetCount;

    //[SerializeField] int maxCastCount;
    //[SerializeField] int CRITDPerCast;
    int remain;
    //int castCount;

    public override void OnBattleStart()
    {
        remain = maxRemain;
    }
    public override void OnRoundStart()
    {
        remain = maxRemain;
    }

    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        if (onAttackParamsList[0].actionStatus.source != this)
        {
            foreach(Action.OnAttackParams onAttackParams in onAttackParamsList)
            {
                if (remain>0&&onAttackParams.hit && onAttackParams.CRIT)
                {
                    remain--;
                    Cast();
                }
            } 
        }
    }

    public override void Cast()
    {
        int targets = 1 + Mathf.FloorToInt((character.CharaStatus().CRITD - 150) / CRITDPerTargetCount);
        Enqueue_SearchTarget(actionStatus, condition, targets);
        //if (castCount < maxCastCount)
        //{
        //    castCount++;
        //    character.AddCRITD(CRITDPerCast);
        //    Log($"CRITダメージ増加(+{castCount * CRITDPerCast}％)");
        //}
    }

    //public override void OnBattleEnd()
    //{
    //    character.AddCRITD(castCount * CRITDPerCast * -1);
    //    castCount = 0;
    //}

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        //\n詠唱した回数：{castCount}/{maxCastCount}(CRITダメージ+{castCount * CRITDPerCast}％)
        return $"残り詠唱回数：{remain}/{maxRemain}\n詠唱の対象数：{1 + Mathf.FloorToInt((character.CharaStatus().CRITD - 150) / CRITDPerTargetCount)}体";
    }
}
