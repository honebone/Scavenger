using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_VampServant : PA_Personality
{
    [SerializeField] int healPercent = 33;
    [SerializeField] Action.ActionStatus actionStatus;
    public override void OnDie(Character killer)
    {
        Action.ActionStatus action = actionStatus;
        action.trueHeal = CharaStatus().summoner.CharaStatus().maxHP.Mul(healPercent);
        Enqueue(action, true, new List<Character> { CharaStatus().summoner });
    }
}
