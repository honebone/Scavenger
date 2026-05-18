using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_PestQueen_Kill : Action
{
    [SerializeField] ActionStatus secondAction;
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    [SerializeField] int killPerEggStack;
    [SerializeField] int valuePerKill;
    public override void SecondEffect()
    {
        int kill = 0;
        foreach (ActionResult result in actionResults)
        {
            if (result.kill && !result.target.IsObstacle()) { kill++; }
        }
        ActionStatus action = secondAction;
        ActionMod.ActionModStatus mod = actionModStatus;

        mod.applyStEBonus[0].exStack = Mathf.FloorToInt(kill / killPerEggStack * 1f);
        mod.applyStEBonus[1].exValue = kill * valuePerKill;

        action = action.Modify(mod);

      if(kill>0)  Enqueue_Self(action);
    }

    public override string GetAdditionalInfo()
    {
        return secondAction.GetInfo();
    }
}
