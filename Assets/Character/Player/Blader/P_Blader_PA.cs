using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Blader_PA : PA_Personality
{
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] Action.ActionStatus combo;

    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        List<Character> target = charactersManager.SearchCharaWithCondition(condition);
        if (target.Count > 0) { Enqueue(attack, true, new List<Character> { target.Choice() }); }
    }

    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        foreach (Action.OnAttackParams attackParams in onAttackParamsList)
        {
            if (attackParams.hit)
            {
                Enqueue_Self(combo);
                break;
            }
        }
    }

   

    public override string GetPAInfo_Base()
    {
        string s = attack.GetInfo(false, new Character.CharacterStatus());
        s += combo.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
