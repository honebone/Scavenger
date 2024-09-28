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
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        count = Mathf.Min(count + 1, maxCount);
    }
    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        count = Mathf.Min(count + onMoveParams.range, maxCount);
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
