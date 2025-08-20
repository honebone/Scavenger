using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_HPPercent_TH : ActionMod
{
    //対象/自身のHPが[THPercent]%以上/以下なら...
    [SerializeField, Header("trueなら以上 falseなら以下")] bool greater;
    [SerializeField, Header("閾値%")] float THPercent;
    [SerializeField, Header("falseなら対象をチェック")] bool checkOwner;
    public bool onlyAttack;

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        float percent = 0;
        Character owner = statusRef.actionOwner;
        Character.CharacterStatus status = checkOwner ? owner.CharaStatus() : new Character.CharacterStatus();

        if (!statusRef.DoesAttack() && onlyAttack) return actionsStatus;

        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if (!checkOwner) status = statusRef.actionTargets[i].CharaStatus();
            percent = status.GetHPPercent();
            if ((!greater && percent > THPercent) || (greater && percent < THPercent)) { break; }
            actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
        }
        return actionsStatus;
    }
}
