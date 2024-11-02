using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_CaramityCane : PA_Equipment
{
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    List<GameObject> applyedStEList = new List<GameObject>();

    public override void OnApplyStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        foreach (Action.OnApplyStEParams onApplyStEParams in onApplyStEParamsList)
        {
            foreach (PA_StatusEffect.StatusEffectParams appliedStE in onApplyStEParams.appliedParams)
            {
                PA_StatusEffect.StatusEffectStatus status = appliedStE.GetStatusEffectStatus();
                if (status.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff && !applyedStEList.Contains(appliedStE.applyStE))
                {
                    applyedStEList.Add(appliedStE.applyStE);
                }
            }
        }
    }

    public override void OnRoundEnd()
    {
        if (applyedStEList.Count > 0)
        {
            List<Character> target = charactersManager.SearchCharaWithCondition(condition);
            if (target.Count > 0)
            {
                for (int i = 0; i < applyedStEList.Count; i++)
                {
                    Enqueue(attack, true, target, 1);
                }
            }
        }
        applyedStEList.Clear();
    }
    public override void OnRoundStart()
    {
        
    }
    public override void OnBattleEnd()
    {
        applyedStEList.Clear();
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += attack.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        string s = $"現在付与したデバフ：{applyedStEList.Count}\n(";
        if (applyedStEList.Count > 0)
        {
            foreach (GameObject obj in applyedStEList) { s += $"{obj.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName} "; }
        }
        s += ")";
        return s;
    }
}
