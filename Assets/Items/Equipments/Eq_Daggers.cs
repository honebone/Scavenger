using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Daggers : PA_Equipment
{
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] int targetValue = 3;
    [SerializeField] int maxRemain = 2;
    int count = 0;
    int remain = 2;

    public override void OnBattleStart()
    {
        count = 0;
        remain = maxRemain;
    }

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn) { remain = maxRemain; }
    }

    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        if (!onAttackParamsList[0].actionParams.actionStatus.abilityEffect && remain > 0)
        {
            count++;
            if (count == targetValue)
            {
                remain--;
                count = 0;

                List<Character> target = charactersManager.SearchCharaWithCondition(condition);
                Enqueue(attack, true, target);
            }
        }
    }

    public override void OnBattleEnd()
    {
        count = 0;
        remain = maxRemain;
    }

    public override string GetPAInfo_Base()
    {
        string s = attack.GetInfo(false, new Character.CharacterStatus());
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        string s = string.Format("現在のカウント数：{0}\n", count);
        s += $"発動可能回数：{remain}";
        return s;
    }
}
