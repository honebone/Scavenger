using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class P_SpiritArcher : PA_Personality
{
    [SerializeField] int CRITCPerDebuff;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] PA_StatusEffect.StatusEffectParams StEParams;
    [SerializeField] ActionMod.ActionModStatus actionModStatus;

    [SerializeField] GameObject trick;
    [SerializeField] Action.ActionStatus action_trick;
    [SerializeField] PA_StatusEffect.StatusEffectParams TrickParams;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        List<GameObject> list = new List<GameObject>();
        if (onDamageParamsList[0].ap.actionStatus.abilityEffect)
        {
            onDamageParamsList.ForEach(p =>
            {
                list.AddRange(p.ap.targetStEs_preResolve.Where(s => s.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff).Select(s => s.StEPrefab));
            });
        }
        list= list.Distinct().ToList();

        Action.ActionStatus action = actionStatus;
        PA_StatusEffect.StatusEffectParams status = StEParams;
        status.stack = list.Count;
        action.applySteParams = new List<PA_StatusEffect.StatusEffectParams>();
        action.applySteParams.Add(status);

        if(list.Count > 0)
        {
            Enqueue_Self(action);
        }
    }

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.DoesAttack())
        {
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                ActionMod.ActionModStatus mod = actionModStatus;
                mod.CRITCMod = CRITCPerDebuff * statusRef.actionTargets[i].GetStEKinds(PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff);
                actionsStatus[i] = actionsStatus[i].Modify(mod);
            }
        }
        return actionsStatus;
    }

    public override void OnSomeoneDied(Character died)
    {
        if (died.GetStEStack_Sum(trick) > 0)
        {
            Action.ActionStatus action = action_trick;
            action.applySteParams = new List<PA_StatusEffect.StatusEffectParams>();
            PA_StatusEffect.StatusEffectParams status = TrickParams;
            status.stack = died.GetStEStack_Sum(trick);
            action.applySteParams.Add(status);

            Enqueue_SearchTarget(action, condition, 1);
        }
    }

    public override string GetPAInfo_Base()
    {
        return actionModStatus.GetModInfo();
    }
}
