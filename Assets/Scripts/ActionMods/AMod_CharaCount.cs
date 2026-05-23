using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_CharaCount : ActionMod
{
    [SerializeField] bool onlyAttack;
    [SerializeField] bool onlyActive;
    [SerializeField] bool onlyPassive;
    [SerializeField] int countTH;
    [SerializeField,Header("false:以上 true:以下")] bool less;
    [SerializeField, Header("false:オーナーを参照 true:対象を参照")] bool refTarget;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (onlyAttack && !statusRef.DoesAttack()) return actionsStatus;
        if (onlyActive && !statusRef.abilityEffect) return actionsStatus;
        if (onlyPassive && statusRef.abilityEffect) return actionsStatus;
        if(!refTarget && !Check(charactersManager.SearchCharaWithCondition(condition, statusRef.actionOwner).Count)) return actionsStatus;

        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if (!refTarget || Check(charactersManager.SearchCharaWithCondition(condition, statusRef.actionTargets[i]).Count))
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
        }
        return actionsStatus;
    }

    bool Check(int count) { return less ? count <= countTH : count >= countTH; }
}
