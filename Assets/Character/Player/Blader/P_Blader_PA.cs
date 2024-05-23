using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Blader_PA : PA_Personality
{
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] int comboTH;
    [SerializeField] Action.ActionStatus combo;
    int comboCount;

    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        List<Character> target = charactersManager.SearchCharaWithCondition(condition);
        Enqueue(attack, true, new List<Character> { target.Choice()});
    }

    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        foreach (Action.OnAttackParams attackParams in onAttackParamsList)
        {
            if (attackParams.hit)
            {
                comboCount++;
                if (comboCount == comboTH)
                {
                    comboCount = 0;
                    Enqueue_Self(combo);
                }
                break;
            }
        }
    }

    public override void OnDamaged(int DMG, Character attacker)
    {
        comboCount = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = attack.GetInfo(true, character.GetCharacterStatus());
        s += combo.GetInfo(true, character.GetCharacterStatus());
        s += string.Format("åªç›{0}òAåÇ", comboCount).ColorStr(Definer.colorRef.currentState);
        return s;
    }
}
