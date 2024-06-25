using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_MecBowgun : PA_Equipment
{
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] int maxRemain = 3;
    int remain = 3;

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn) { remain = maxRemain; }
    }

    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        List<Character> target = charactersManager.SearchCharaWithCondition(condition);
        if (target.Count > 0 && remain > 0)
        {
            foreach (Action.OnAttackParams onAttackParams in onAttackParamsList)
            {
                if (onAttackParams.CRIT)
                {
                    if (remain > 0)
                    {
                        remain--;
                        Enqueue(attack, true, target.Sample(1));
                    }
                }
                if (remain == 0) { break; }
            }
        }
    }

    public override void OnBattleEnd()
    {
        remain = maxRemain;
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += attack.GetInfo(false, new Character.CharacterStatus());
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        return string.Format("écÇËî≠ìÆâÒêîÅF{0}", remain);
    }
}
