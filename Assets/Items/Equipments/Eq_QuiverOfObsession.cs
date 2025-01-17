using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_QuiverOfObsession : PA_Equipment
{
    [SerializeField] int ACCPerHit;
    [SerializeField] int maxHit;
    [SerializeField] float exDMGPerACC;
    [SerializeField] float maxExDMG;
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    int currentHit;
    int hitCount;

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.DoesAttack())
        {
            actionModStatus.exDMG_mul = Mathf.Clamp((character.CharaStatus().ACC - 100) * exDMGPerACC, 0, maxExDMG);
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }

        return actionsStatus;
    }

    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        foreach(Action.OnAttackParams onAttackParams in onAttackParamsList)
        {
            if (onAttackParams.hit)
            {
                hitCount++;
                hitCount = hitCount.Limit(maxHit);
            }
        }
        RefreshValue();
    }

    void RefreshValue()
    {
        if (hitCount > currentHit) { Log($"ACC増加({hitCount * ACCPerHit})"); }
        character.AddACC(currentHit * ACCPerHit * -1);
        character.AddACC(hitCount * ACCPerHit);
        currentHit = hitCount;
    }

    public override void OnBattleEnd()
    {
        hitCount = 0;
        RefreshValue();
    }

    public override string GetCurrentStateInfo()
    {
        string info = $"攻撃命中回数：{hitCount}/{maxHit}(ACC+{hitCount * ACCPerHit})\n";
        info += $"与ダメージ+{Mathf.Clamp((character.CharaStatus().ACC - 100) * exDMGPerACC, 0, maxExDMG)}(最大{maxExDMG}％)";
        return info;
    }
}
