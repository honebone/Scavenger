using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_AbyssHand : PA_Equipment
{
    [SerializeField] GameObject actionMod;
    [SerializeField] List<Vector2Int> neighbor;
    [SerializeField] int maxCount;
    int count;
    bool activated;

    public override void OnSomeoneApplyedStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        if (!activated)
        {
            bool f = false;
            Character.CharacterStatus status = character.GetCharacterStatus();
            foreach (Action.OnApplyStEParams onApplyStEParams in onApplyStEParamsList)
            {
                if (charactersManager.GetCharactersWithPos(status.position.RelativePosToAbsolute(neighbor)).Contains(onApplyStEParams.taget))
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
                    count++;
                    break;
                }
            }

            if (count == maxCount)
            {
                count = 0;
                activated = true;
                character.AddActionMod(actionMod, true);
            }
        }
    }

    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        if (onAttackParamsList[0].actionStatus.abilityEffect && activated)
        {
            activated = false;
            character.AddActionMod(actionMod, false);
        }
    }

    public override void OnBattleEnd()
    {
        count = 0;
        if (activated)
        {
            activated = false;
            character.AddActionMod(actionMod, false);
        }
    }
    public override string GetCurrentStateInfo()
    {
        return (activated) ? "能力発動中" : $"現在カウント：{count}/{maxCount}";
    }
}
