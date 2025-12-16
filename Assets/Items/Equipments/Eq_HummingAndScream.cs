using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_HummingAndScream : PA_Equipment
{
    [SerializeField] int exDMGPerCount;
    [SerializeField] int maxCount;
    [SerializeField] ActionMod.ActionModStatus mod_empty;
    [SerializeField] ActionMod.ActionModStatus mod_active;
    [SerializeField] ActionMod.ActionModStatus mod_passive;

    bool scream;
    int count;
    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        if (onAttackParamsList[0].actionParams.actionStatus.abilityEffect)
        {
            if (scream)
            {
                AddCount();
                scream = false;
                Log("鼻唄");
            }
        }
        else
        {
            if (!scream)
            {
                AddCount();
                scream = true;
                Log("絶叫");
            }
        }
    }

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        ActionMod.ActionModStatus mod = new ActionMod.ActionModStatus();
        bool f = false;
        if (statusRef.DoesAttack())
        {
            if (statusRef.abilityEffect && scream)
            {
                f = true;
                mod = count == maxCount ? mod_active : mod_empty;
            }
            else if (!(statusRef.abilityEffect || scream))
            {
                f = true;
                mod = count == maxCount ? mod_passive : mod_empty;
            }

            if (f)
            {
                for (int i = 0; i < actionsStatus.Length; i++)
                {
                    mod.exDMG_mul = count * exDMGPerCount;
                    actionsStatus[i] = actionsStatus[i].Modify(mod);
                }
            }
        }
        return actionsStatus;
    }

    void AddCount()
    {
        if (count < maxCount)
        {
            Log("カウント+1");
            count++;
        }
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        string s = $"{(scream ? "アビリティ" : "誘発能力".ToLinkKey())}による攻撃の与ダメージ+{count * exDMGPerCount}％";
        return $"カウント：{count}/{maxCount}\n現在の状態：{(scream ? "絶叫" : "鼻唄")}\n{s}";
    }
}
