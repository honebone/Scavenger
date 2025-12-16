using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_DebuffKinds : ActionMod
{
    [Header("対象/自身のデバフ1種類につき")]
    public int maxCount;
    public bool checkSelf;
    public bool onlyAttack;
    public bool onlyAlly;
    [SerializeField, Header("オーナーから見て敵！")] bool onlyOpponent;
    public bool onlyAbility;
    public bool onlyPassive;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (!statusRef.DoesAttack() && onlyAttack) return actionsStatus;
        Character chara = statusRef.actionOwner;
        if (!statusRef.abilityEffect && onlyAbility) return actionsStatus;
        if (statusRef.abilityEffect && onlyPassive) return actionsStatus;

        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            Character target = statusRef.actionTargets[i];
            if (statusRef.actionOwner.PlayerPos() == statusRef.actionTargets[i].PlayerPos()) { if (onlyOpponent) continue; }
            else if (onlyAlly) continue;

            if (!checkSelf) chara = statusRef.actionTargets[i];
            int count = chara.GetStEKinds(PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff);
            if (maxCount > 0) count = count.Limit(maxCount);
            InfoText.inst.AddDebugText($"{count}");
            for (int j = 0; j < count; j++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }

        return actionsStatus;
    }
}
