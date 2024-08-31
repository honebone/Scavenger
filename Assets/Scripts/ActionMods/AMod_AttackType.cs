using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_AttackType : ActionMod
{
   [SerializeField] bool checkATK;
  [SerializeField]  bool checkINT;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (statusRef.DoesAttack())
        {
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                if ((statusRef.ATKMod_max > 0 && checkATK) || (statusRef.INTMod_max > 0 && checkINT))
                {
                    actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
                }
            }
        }
      
        return actionsStatus;
    }
}
