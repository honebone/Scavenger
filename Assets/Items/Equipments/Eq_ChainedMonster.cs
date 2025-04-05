using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_ChainedMonster : PA_Equipment
{
    public int HPTH;
    public int reqHPHeal;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] Action.ActionStatus actionStatus_self;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    bool available;
    int healCount;
    public override void OnBattleStart()
    {
        available = true;
    }
    public override void OnDecreasedHP(int value)
    {
        if (available && character.CharaStatus().GetHPPercent() <= HPTH)
        {
            available = false;
            Enqueue_Self(actionStatus_self);
            Enqueue_SearchTarget(actionStatus, condition);
        }
    }
    public override void OnHealed(Character healer, Action.OnHealParams onHealParams)
    {
        if (!available)
        {
            healCount += onHealParams.healValue;
            if (healCount >= (character.CharaStatus().maxHP * reqHPHeal / 100f).ToInt())
            {
                available = true;
                healCount = 0;
            }
        }
    }
    public override void OnBattleEnd()
    {
        available = true;
        healCount = 0;
    }
    public override string GetCurrentStateInfo()
    {
        return available ? "発動可能" : $"回復量/必要量：{healCount}/{(character.CharaStatus().maxHP * reqHPHeal / 100f).ToInt()}";
    }
    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus_self.GetInfo();
        s += "\n"+actionStatus.GetInfo();
        return s;
    }
}
