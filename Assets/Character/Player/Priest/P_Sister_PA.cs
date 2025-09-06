using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Sister_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField]
    GameObject comeTrue;
    [SerializeField, TextArea(3, 10)]
    string comeTrueInfo;

    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        if (actionResultsList[0].actionStatus.abilityType != AbilityData.AbilityType.move && !character.CheckHasStE(comeTrue))
        {
            Action.ActionStatus action = actionStatus;
            character.Enqueue(action, true, new List<Character>() { character });
        }
    }

    public override void OnHeal(List<Action.OnHealParams> onHealParamsList)
    {
        bool f = false;
        if (!character.CheckHasStE(comeTrue))
        {
            foreach(Action.OnHealParams onHealParams in onHealParamsList)
            {
                if (onHealParams.target != character)
                {
                    f = true;
                    break;
                }
            }
            if (f)
            {
                Action.ActionStatus action = actionStatus;
                character.Enqueue(action, true, new List<Character>() { character });
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s= actionStatus.GetInfo(false, new Character.CharacterStatus());
        s += comeTrueInfo;
        return s;
    }
}
