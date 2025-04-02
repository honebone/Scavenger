using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_OnDebuff : ActionMod
{
    [SerializeField] bool onlyAlly;
    [SerializeField,Header("オーナーから見て敵！")] bool onlyEnemy;
    [SerializeField] bool onlyAbility;
    [SerializeField] bool onlyPassive;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        Character owner = statusRef.actionOwner;
        if (!statusRef.abilityEffect && onlyAbility) return actionsStatus;
        if (statusRef.abilityEffect && onlyPassive) return actionsStatus;

        bool f = false;
        foreach(PA_StatusEffect.StatusEffectParams StE in statusRef.applySteParams)
        {
            if (StE.GetStatusEffectStatus().StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff)
            {
                f = true;
                break;
            }
        }

        if (f)
        {
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                Character target = statusRef.actionTargets[i];
                if (owner.PlayerPos() == target.PlayerPos()) { if (onlyEnemy) continue; }
                else if (onlyAlly) continue;

                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        
        return actionsStatus;
    }
}
