using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_EnergyArmor : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField] int maxCount;
    [SerializeField] int valuePerCount;
    int count;

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (count < maxCount)
        {
            count++;
            Log($"カウント+1 ({count})");
        }
    }
    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        if(count < maxCount)
        {
            count = Mathf.Min(count + onMoveParams.range, maxCount);
            Log($"カウント+{onMoveParams.range} ({count})");
        }
    }
    public override void OnRoundEnd()
    {
        if (count > 0)
        {
            Action.ActionStatus action = actionStatus;
            action.shieldPercent_min = valuePerCount * count;
            action.shieldPercent_max = valuePerCount * count;
            Enqueue_Self(action);

            count = 0;
        }
    }
    public override void OnBattleEnd()
    {
        count = 0;
    }
    public override string GetCurrentStateInfo()
    {
        return $"現在のカウント：{count}/{maxCount}";
    }
}
