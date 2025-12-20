using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SoulOfWish : PA_Personality
{
    public int countOnMyTurn;
    //[SerializeField] Action.ActionStatus exTurn;
    //[SerializeField] CharactersManager.SearchCharaCondition players;
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition enemies;
    int count;
    //public override void OnSummoned(Action.OnSummonParams onSummonParams)
    //{
    //    Enqueue_SearchTarget(exTurn, players);    
    //}

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.turnChara.IsPlayer())
        {
            count++;
            Log($"カウント+1 ({count})");
        }
        if (tep.turnChara == character)
        {
            count += countOnMyTurn;
            Log($"カウント+{countOnMyTurn} ({count})");
        }
    }

    public override void OnRoundEnd()
    {
        for (int i = 0; i < count; i++) { Enqueue_SearchTarget(attack, enemies,1); }
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        return $"{attack.GetInfo()}";
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}";
    }
}
