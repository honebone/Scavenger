using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Zombie : PA_Personality
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] int ratio;
    public override void OnRoundEnd()
    {
        Action.ActionStatus action = actionStatus;
        int HP = Mathf.CeilToInt(character.CharaStatus().BaseHP() * ratio / 100f);
        action.decreaseHP_min = HP;
        action.decreaseHP_max = HP;

        Enqueue_Self(action);
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo(false, new Character.CharacterStatus());
    }
}
