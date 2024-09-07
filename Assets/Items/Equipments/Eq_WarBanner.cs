using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_WarBanner : PA_Equipment
{
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    int count;
    public override void OnApplyStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        foreach(Action.OnApplyStEParams onApplyStEParams in onApplyStEParamsList)
        {
            foreach(PA_StatusEffect.StatusEffectParams appliedStE in onApplyStEParams.appliedParams)
            {
                PA_StatusEffect.StatusEffectStatus status = appliedStE.GetStatusEffectStatus();
                if (status.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff)
                {
                    count++;
                    break;
                }
            }
        }
    }

    public override void OnRoundEnd()
    {
        for(int i = 0; i <= count; i++)
        {
            List<Character> target = charactersManager.SearchCharaWithCondition(condition);
            Enqueue(attack, true, new List<Character> { target.Choice() });
        }
        count = 0;
    }
    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += attack.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return string.Format("現在のカウント数：{0}", count);
    }
}
