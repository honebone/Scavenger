using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StEU_Mount : PA_StatusEffect
{
    public Action.ActionStatus onApplied;

    public override void OnPAInit()
    {
        Enqueue_Self(onApplied);
    }

    public override void OnDecreasedHP(int value)
    {
        if (character.CharaStatus().GetHPPercent() < 50)
        {
            Disable();
        }
    }

    public override void AtTheEnd()
    {
        //character.SetCharaSprite()
    }

    public override string GetAdditionalInfo()
    {
        return onApplied.GetInfo();
    }
}
