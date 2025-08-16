using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionMod;

public class Eq_BloodCutlass : PA_Equipment
{
    public int DMGPercent;
    public int drainPercent;
    [Header("maxHPの何パーセントを上限とするか")]
    public int drainLimitPercent;

    public GameObject bleed;
    public Action.ActionStatus actionStatus;
    public ActionMod.ActionModStatus actionMod;

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.DoesAttack() && statusRef.abilityEffect)
        {
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                int DMG = (statusRef.actionTargets[i].GetDoTDMG(bleed, false) * DMGPercent / 100f).ToInt();

                if (DMG > 0)
                {
                    ActionMod.ActionModStatus mod = actionMod;
                    mod.trueINTDMG = DMG;
                    actionsStatus[i] = actionsStatus[i].Modify(mod);
                }
            }
        }
       
        return actionsStatus;
    }

    public override void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        PA_StatusEffect bleedPA = bleed.GetComponent<PA_StatusEffect>();
        int heal = 0;
        foreach (var list in onDamageParamsList)
        {
            int value = 0;
            List<PA_StatusEffect.StatusEffectStatus> bleeds = list.ap.targetStEs_preResolve.SampleStE(bleedPA);
            infoText.AddDebugText(bleeds.Count.ToString());
            foreach (var b in bleeds)
            {
                value += b.DMGPerTurn;
            }
            if(value > 0)
            {
                value = Mathf.Min((value * drainPercent / 100f).ToInt(), (character.CharaStatus().maxHP * drainLimitPercent / 100f).ToInt());
                heal += value;
            }
        }
        if (heal > 0)
        {
            infoText.AddDebugText(heal.ToString());
            Action.ActionStatus action = actionStatus;
            action.healValue_min = heal;
            action.healValue_max = heal;
            Enqueue_Self(action);
        }
    }
}
