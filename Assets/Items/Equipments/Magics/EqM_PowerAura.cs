using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_PowerAura : Eq_Magic
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    //[SerializeField] int maxRemain = 8;

    //int remain;
    //public override void OnBattleStart()
    //{
    //    remain = maxRemain;
    //}
    //public override void OnRoundStart()
    //{
    //    remain = maxRemain;
    //}

    public override void OnSummon(List<Action.OnSummonParams> onSummonParamsList)
    {
        Cast();
    }

    public override void Cast()
    {
        List<Character> targets = charactersManager.SearchCharaWithCondition(condition);
        if (Enqueue(actionStatus, true, targets)) character.OnCast(this);
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }

    //public override string GetCurrentStateInfo()
    //{
    //    return $"écÇËârè•âÒêîÅF{remain}/{maxRemain}";
    //}
}
