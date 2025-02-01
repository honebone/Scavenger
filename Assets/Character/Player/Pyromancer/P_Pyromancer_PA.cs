using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Pyromancer_PA : PA_Personality
{
    //[SerializeField, TextArea(3, 10)]
    //string info;
    //[SerializeField] Action.ActionStatus attack;
    //[SerializeField] CharactersManager.SearchCharaCondition condition;

    //public override void OnRoundStart()
    //{
    //    if (character.GetCharacterStatus().CRITC.Dice())
    //    {
    //        List<Character> target = charactersManager.SearchCharaWithCondition(condition);
    //        if (target.Count > 0)
    //        {
    //            Enqueue(attack, true, target);
    //        }
    //    }
    //}

    //public override string GetPAInfo_Base()
    //{
    //    string s = info+"\n\n";
    //    s += attack.GetInfo(false, new Character.CharacterStatus());
    //    return s;
    //}

    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] StEApplyBonus stEApplyBonus;
    [SerializeField] CharactersManager.SearchCharaCondition bunred;
    [SerializeField] GameObject burn;

    [SerializeField] GameObject ember;

    //public override void OnBattleStart()
    //{
    //    Enqueue_Self(actionStatus);
    //}

    //public override void OnRoundStart()
    //{
    //    Enqueue_Self(actionStatus);
    //}

    public override void OnSomeoneApplyedStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        int stack = 0;
        foreach (Action.OnApplyStEParams onApplyStEParams in onApplyStEParamsList)
        {
            if (!onApplyStEParams.taget.CharaStatus().position.IsPlayerPos())
            {
                foreach (PA_StatusEffect.StatusEffectParams statusEffectParams in onApplyStEParams.appliedParams)
                {
                    if (statusEffectParams.applyStE == burn)
                    {
                        stack++;
                        break;
                    }
                }
            }
        }

        if (stack > 0)
        {
            Action.ActionStatus action = actionStatus;
            StEApplyBonus bonus = new StEApplyBonus(stEApplyBonus);
            bonus.exStack = stack;
            action.StEApplyBonus = new List<StEApplyBonus> { bonus };

            Enqueue_Self(action);
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        //s += "\n"+attack.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
