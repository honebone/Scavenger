using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Sister_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField, TextArea(3, 10)]
    string comeTrueInfo;

    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        if (actionResultsList[0].actionStatus.abilityType != AbilityData.AbilityType.move)
        {
            Enqueue_Self(actionStatus);
        }
    }

    public override void OnHeal(List<Action.OnHealParams> onHealParamsList)
    {
        foreach (Action.OnHealParams onHealParams in onHealParamsList)
        {
            if (onHealParams.target != character)
            {
                Enqueue_Self(actionStatus);
                break;
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo(false, new Character.CharacterStatus());
        s += "\n"+comeTrueInfo;
        return s;
    }
}
