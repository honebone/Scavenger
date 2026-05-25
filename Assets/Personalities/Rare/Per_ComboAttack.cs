using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_ComboAttack : PA_Personality
{
    [SerializeField] Action.ActionStatus actionStatus;
    public override void OnSomeoneDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        if (onDamageParamsList.Count != 1 || onDamageParamsList[0].ap.owner.GetRootChara() != character || onDamageParamsList[0].ap.owner == character) return;
        Enqueue(actionStatus,true,new List<Character> { onDamageParamsList[0].ap.target });
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }
}
