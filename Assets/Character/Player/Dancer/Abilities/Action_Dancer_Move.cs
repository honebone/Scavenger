using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Dancer_Move : Action
{
    [SerializeField] ActionStatus secondAction;
    public override void SecondEffect()
    {
        actionResults.ForEach(a =>
        {
            if (a.move && a.onMoveParams.passedBy.Count > 0)
            {
                Enqueue(secondAction, true, a.onMoveParams.passedBy);
            }
        });
    }

    public override string GetAdditionalInfo()
    {
        return secondAction.GetInfo();
    }
}
