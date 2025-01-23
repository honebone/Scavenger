using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_GauntletOfProsperity : PA_Equipment
{
    [SerializeField] int maxCount;
    [SerializeField] int maxHPPerCount;
    [SerializeField] int DMGRatio;
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    int count;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.DoesAttack() && statusRef.abilityEffect)
        {
            ActionMod.ActionModStatus mod = actionModStatus;
            mod.exINTDMG_int = (character.CharaStatus().maxHP * DMGRatio / 100f).ToInt();
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                if (statusRef.actionTargets[i].CharaStatus().focused > 0)
                {
                    actionsStatus[i] = actionsStatus[i].Modify(mod);
                }
            }
        }

        return actionsStatus;
    }

    public override void OnFocus(List<Action.OnFocusParams> focusParamsList)
    {
        for (int i = 0; i < focusParamsList.Count; i++)
        {
            if (count < maxCount)
            {
                count++;
                Log($"カウント増加({count})");
                character.AddMaxHP(0, maxHPPerCount,true);
            }
        }
    }

    public override void OnBattleEnd()
    {
        character.AddMaxHP(0, -maxHPPerCount * count,false);
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += "\n" + actionModStatus.GetModInfo();
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        return $"現在のカウント：{count}/{maxCount}";
    }
}
