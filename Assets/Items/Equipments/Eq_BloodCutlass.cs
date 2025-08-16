using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionMod;

public class Eq_BloodCutlass : PA_Equipment
{
    public int DMGPercent;
    public int drainPercent;
    [Header("ó^É_ÉÅÅ[ÉWÇÃâΩ%Çè„å¿Ç∆Ç∑ÇÈÇ©")]
    public int drainLimitPercent;

    public GameObject bleed;
    public Action.ActionStatus actionStatus;
    public ActionMod.ActionModStatus actionMod;

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
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
        return actionsStatus;
    }

    public override void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        PassiveAbility bleedPA = bleed.GetComponent<PassiveAbility>();
        int heal = 0;
        foreach (var list in onDamageParamsList)
        {
            int value = 0;
            List<PassiveAbility> bleeds = list.ap.targetStEs_preResolve.SampleStE(bleedPA);
            foreach(var b in bleeds)
            {
                value += b.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().DMGPerTurn;
            }
            if(value > 0)
            {
                value = Mathf.Min((value * drainPercent / 100f).ToInt(), (list.totalDMG * drainLimitPercent / 100f).ToInt());
                heal += value;
            }
        }
        if (heal > 0)
        {
            Action.ActionStatus action = actionStatus;
            action.healValue_min = heal;
            action.healValue_max = heal;
            Enqueue_Self(action);
        }
    }
}
