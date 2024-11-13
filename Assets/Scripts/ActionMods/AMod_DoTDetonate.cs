using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_DoTDetonate : ActionMod
{
    [SerializeField] bool ATKDMG;
    [SerializeField] List<GameObject> DoT;
    [SerializeField] float ratio;

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            int DMG = 0;
            foreach(GameObject d in DoT)
            {
                DMG += statusRef.actionTargets[i].GetDoTDMG(d, true);
            }

            if (ATKDMG) { actionModStatus.trueATKDMG = (DMG * ratio).ToInt(); }
            else { actionModStatus.trueINTDMG = (DMG * ratio).ToInt(); }
            actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
        }
        return actionsStatus;
    }
}
