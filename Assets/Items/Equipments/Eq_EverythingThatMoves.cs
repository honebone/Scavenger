using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_EverythingThatMoves : PA_Equipment
{
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] int maxRemain;
    int remain = 3;

    public override void OnRoundStart()
    {
        remain = maxRemain;
    }

    public override void OnSomeoneMove(Action.OnMoveParams onMoveParams)
    {
        if (remain > 0 && !onMoveParams.secondaryMove)
        {
            if (charactersManager.ExamineCharacter(onMoveParams.target, condition))
            {
                Enqueue(attack, true, new List<Character> { onMoveParams.target });
                remain--;
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
        return string.Format("c‚è”­“®‰ñ”F{0}", remain);
    }
}
