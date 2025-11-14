using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_BloodVessel : PA_Equipment
{
    public int gainMaxHPPercent;
    public int HPTH;
    public float exDMGPerHPLoss;
    public Action.ActionStatus action;
    //ActionMod.ActionModStatus actionMod;

    int gainMaxHP;
    public override void OnBattleStart()
    {
        gainMaxHP = character.CharaStatus().maxHP;
        character.AddMaxHP(0, 0, false, gainMaxHP);
        Log($"{"maxHP".ToSpr_withName()}+{gainMaxHP}");
    }

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.DoesAttack() && statusRef.abilityEffect)
        {
            float exDMG = exDMGPerHPLoss * (100 - character.CharaStatus().GetHPPercent()).ToInt();
            if (exDMG > 0)
            {
                //ActionMod.ActionModStatus mod = actionMod;
                //mod.exINTDMG_int = exDMG;

                //for (int i = 0; i < actionsStatus.Length; i++) { actionsStatus[i] = actionsStatus[i].Modify(mod); }
                for (int i = 0; i < actionsStatus.Length; i++) { actionsStatus[i].exDMG_mul += exDMG; }
            }
        }

        return actionsStatus;
    }

    public override void OnRoundStart()
    {
        if (character.CharaStatus().GetHPPercent() > HPTH)
        {
            Action.ActionStatus a = action;
            int setHP = (character.CharaStatus().maxHP * HPTH / 100f).ToInt();
            a.decreaseHP_max = character.CharaStatus().HP - setHP;
            a.decreaseHP_min = character.CharaStatus().HP - setHP;
            Enqueue_Self(a);
        }
    }

    public override void OnBattleEnd()
    {
        character.AddMaxHP(0, 0, false, -gainMaxHP);
        gainMaxHP = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"アビリティ与ダメージ+{exDMGPerHPLoss * (100 - character.CharaStatus().GetHPPercent()).ToInt()}％";
    }
}
