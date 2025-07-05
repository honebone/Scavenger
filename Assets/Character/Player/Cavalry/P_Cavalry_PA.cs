using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Cavalry_PA : PA_Personality
{
    public Action.ActionStatus onBS;

    public override void OnBattleStart()
    {
        if (character.CharaStatus().GetHPPercent() >= 50)
        {
            Enqueue_Self(onBS);
        }
    }

    public override string GetPAInfo_Base()
    {
        return onBS.GetInfo();
    }
}
