using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Horseshoe : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] int valueLossPerTurn;
    [SerializeField] int defValue;
    [SerializeField] PA_StatusEffect.StatusEffectParams ATK;
    [SerializeField, TextArea(3, 10)] string exInfo;

    bool f;
    int currentValue;

    public override void OnBattleStart()
    {
        f = false;
        currentValue = defValue;
    }

    public override void OnRoundEnd()
    {
        f = false;
        currentValue = defValue;
    }

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (f)
        {
            currentValue = Mathf.Max(0, currentValue - valueLossPerTurn);
        }
        f = true;

        if (myTurn && currentValue>0)
        {
            Action.ActionStatus action = actionStatus;
            PA_StatusEffect.StatusEffectParams apply = ATK;
            apply.value = currentValue;
            action.applySteParams.Add(apply);

            Enqueue_Self(action);
            actionStatus.applySteParams = new List<PA_StatusEffect.StatusEffectParams>();
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo(false, new Character.CharacterStatus());
        s += ATK.GetInfo();
        s += exInfo;
        return s;
    }
}
