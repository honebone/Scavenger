using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_StealHammer : PA_Equipment
{
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    [SerializeField] float HPRatio;
    public int maxCount;
    int count;
   
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (onDamageParams.totalDMG > 0&&count<maxCount) {
            count++;
            Log($"カウント+1 (+{count})");
        }
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (count > 0 && statusRef.DoesAttack())
        {
            ActionMod.ActionModStatus mod = actionModStatus;
            float DMGF = character.CharaStatus().maxHP * HPRatio * count;
            mod.exATKDMG_int = (DMGF / actionsStatus.Length).ToInt();
            infoText.AddDebugText($"exDMG:{mod.exATKDMG_int}");

            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(mod);
            }
            if (!forCalcDMG) { count = 0; }
        }

        return actionsStatus;
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}/{maxCount}\n次の追加ダメージ：{(character.CharaStatus().maxHP * HPRatio * count).ToInt()}";
    }
}
