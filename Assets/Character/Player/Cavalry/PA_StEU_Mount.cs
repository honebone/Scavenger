using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StEU_Mount : PA_StatusEffect
{
    public GameObject human;
    public GameObject horse;
    public Action.ActionStatus onApplied;

    public override void OnPAInit()
    {
        Enqueue_Self(onApplied);
        character.SetCharaSprite(horse);
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
        character.SetCharaSprite(human);
    }

    public override string GetAdditionalInfo()
    {
        return onApplied.GetInfo();
    }
}
