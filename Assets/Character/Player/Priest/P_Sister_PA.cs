using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Sister_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField]
    GameObject comeTrue;
    
    public override void OnActivateAbility()
    {
        if (!character.CheckHasStE(comeTrue))
        {
            Action.ActionStatus action = actionStatus;
            character.Enqueue(action, true, new List<Character>() { character });
        }
    }

    public override void OnHeal(List<Action.OnHealParams> onHealParamsList)
    {
        if (!character.CheckHasStE(comeTrue))
        {
            Action.ActionStatus action = actionStatus;
            character.Enqueue(action, true, new List<Character>() { character });
        }
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo(true, character.GetCharacterStatus());
    }
}
