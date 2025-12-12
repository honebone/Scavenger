using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_AbyssHand : PA_Equipment
{
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    [SerializeField] List<Vector2Int> neighbor;
    [SerializeField] int maxCount;
    [SerializeField] int countGoal;
    int count;
    bool activated;

    public override void OnSomeoneApplyedStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        bool f = false;
        Character.CharacterStatus status = character.CharaStatus();
        foreach (Action.OnApplyStEParams onApplyStEParams in onApplyStEParamsList)
        {
            if (charactersManager.GetCharactersWithPos(status.position.RelPosToAbs(neighbor)).Contains(onApplyStEParams.taget))
            {
                foreach (PA_StatusEffect.StatusEffectParams statusEffectParams in onApplyStEParams.appliedParams)
                {
                    if (statusEffectParams.GetStatusEffectStatus().StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff)
                    {
                        f = true;
                        break;
                    }
                }
            }
            if (f)
            {
                if (count < maxCount)
                {
                    count++;
                    Log($"カウント+1 ({count}/{maxCount})");
                }
                break;
            }
        }

        if (count >= countGoal)
        {
            activated = true;
            //character.AddActionMod(actionMod, true);
        }
    }

    //public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    //{
    //    if (onAttackParamsList[0].actionStatus.abilityEffect && activated)
    //    {
    //        activated = false;
    //        character.AddActionMod(actionMod, false);
    //    }
    //}

    public override void OnBattleEnd()
    {
        count = 0;
        if (activated)
        {
            activated = false;
            //character.AddActionMod(actionMod, false);
        }
    }

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (activated && statusRef.DoesAttack() && statusRef.abilityEffect)
        {
            if (!forCalcDMG)
            {
                count -= 4;
                activated = count >= countGoal;
            }
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
    
        return actionsStatus;
    }
    public override string GetCurrentStateInfo()
    {
        return $"現在カウント：{count}/{countGoal}";
    }
}
