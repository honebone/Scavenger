using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_Heal : Eq_Magic
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    [SerializeField] int CDRound;
    [SerializeField] int TH;
    int CD;
    bool available;
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

    public override void OnSomeoneDamaged(Action.OnDamageParams onDamageParams)
    {
        Character target = onDamageParams.target;

        if (available && target.IsPlayer() && target.CharaStatus().GetHPPercent() <= TH)
        {
            available = false;
            CD = CDRound;

            Cast();
        }
    }

    public override void Cast()
    {
       if( Enqueue(actionStatus, true, charactersManager.SearchChara_Weakest(condition, true))) character.OnCast(this);
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
        return (available) ? "詠唱可能" : $"再詠唱まで{CD}ラウンド";
    }
}
