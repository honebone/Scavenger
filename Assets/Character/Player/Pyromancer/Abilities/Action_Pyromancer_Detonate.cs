using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Pyromancer_Detonate : Action
{
    [SerializeField] ActionStatus secondAction;
    public override void SecondEffect()
    {
        int DMG = 0;
        foreach(ActionResult result in actionResults)
        {
            DMG += result.onDamageParams.totalDMG;
        }
        int shield = Mathf.Min((DMG * actionOwner.GetCharacterStatus().CRITC / 100f).ToInt(), actionOwner.GetCharacterStatus().maxHP);
        ActionStatus action = secondAction;
        action.shieldAdd_min = shield;
        action.shieldAdd_max = shield;

        Enqueue_Self(action);
    }
}
