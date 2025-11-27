using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StEU_Commander_Trained : PA_StatusEffect
{
    [SerializeField] ActionMod.ActionModStatus actionModStatus;
    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.DoesAttack() && statusRef.abilityEffect)
        {
            List<int> focusIndex = new List<int>();
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                if (statusRef.actionTargets[i].CharaStatus().focused > 0)
                {
                    focusIndex.Add(i);
                }
            }

            focusIndex = focusIndex.Sample(StEStatus.stack);
            foreach (int i in focusIndex)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }

            if(!forCalcDMG) AddStack(focusIndex.Count * -1);
        }

        return actionsStatus;
    }

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn && applyFlag) { AddStack(-1); }
    }

    public override string GetAdditionalInfo()
    {
        string s = "";
        s += actionModStatus.GetModInfo();
        return s;
    }
}
