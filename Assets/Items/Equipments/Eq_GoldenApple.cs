using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_GoldenApple : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    public override void OnBattleStart()
    {
        Action.ActionStatus action = actionStatus;
        action.trueHeal = character.CharaStatus().maxHP;
        Enqueue_Self(action); 
    }

    public override void OnBattleEnd()
    {
        character.UnequipItem(this, false);
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }
}
