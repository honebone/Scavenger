using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Warrior_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField, TextArea(3, 10)] string battleStart;

    public override void OnBattleStart()
    {
        float decreased = 100f - character.CharaStatus().GetHPPercent();
        while (decreased >= 10f)
        {
            decreased -= 10f;
            Action.ActionStatus action = actionStatus;
            Enqueue_Self(action);
        }
    }
    public override void OnDecreasedHP(int value)
    {
        Action.ActionStatus action = actionStatus;
        Enqueue_Self(action);
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo(false, new Character.CharacterStatus());
        s += battleStart;
        return s;
    }
}
