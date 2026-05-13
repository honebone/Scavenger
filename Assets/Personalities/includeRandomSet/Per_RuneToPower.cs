using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Per_RuneToPower : PA_Personality
{
    [SerializeField] int ATKModPerActivate;
    [SerializeField] ActionMod.ActionModStatus actionMod;
    int count;
    int ATKDiv => count * ATKModPerActivate;

    public override void OnRuneActivate(PassiveAbility rune)
    {
        count++;
        LogCount(count);
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        if (onDamageParamsList == null || onDamageParamsList.Count == 0) return;
        if (onDamageParamsList[0].ap.actionStatus.abilityEffect) count = 0;
    }

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if(!statusRef.abilityEffect||!statusRef.DoesAttack())return actionsStatus;

        ActionMod.ActionModStatus mod = actionMod;
        mod.ATKDMG_divide_mul = ATKDiv;
        for (int i = 0; i < actionsStatus.Length; i++)
        {
            actionsStatus[i] = actionsStatus[i].Modify(mod);
        }

        return actionsStatus;
    }

    public override string GetPAInfo_Base()
    {
        return actionMod.GetModInfo();
    }


    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count} \n(分配{"ATK".ToSpr_withName()}補正：{ATKDiv}％)";
    }
}
