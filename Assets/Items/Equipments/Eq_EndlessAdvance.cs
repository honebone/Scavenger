using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_EndlessAdvance : PA_Equipment
{
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    [SerializeField] int DMGPerCount;

    int count;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (count >= 2 && statusRef.DoesAttack())
        {
            ActionMod.ActionModStatus mod = actionModStatus;
            float exDMG = DMGPerCount * count;
            mod.exDMG_mul = exDMG;
            Debug.Log(mod.exDMG_mul);
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(mod);
            }
        }

        return actionsStatus;
    }

    public override void OnBattleStart()
    {
        count = 0;
    }
    public override void OnRoundEnd()
    {
        count = 0;
    }

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn) { count++; }
    }

    public override string GetCurrentStateInfo()
    {
        int exDMG = (count >= 2) ? count*DMGPerCount : 0;
        return $"カウント{count}(与ダメージ+{exDMG}％)";
    }
}
