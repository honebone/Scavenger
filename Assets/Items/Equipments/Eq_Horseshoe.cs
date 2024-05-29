using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Horseshoe : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] int valuePerACT;
    [SerializeField] int maxStack;
    [SerializeField] PA_StatusEffect.StatusEffectParams ATK;
    [SerializeField, TextArea(3, 10)] string exInfo;

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn && character.GetCharacterStatus().ACT > 10)
        {
            int value = Mathf.Clamp(character.GetCharacterStatus().ACT - 10, 0, maxStack);
            Action.ActionStatus action = actionStatus;
            PA_StatusEffect.StatusEffectParams apply = ATK;
            apply.value = value * valuePerACT;
            action.applySteParams.Add(apply);

            Enqueue_Self(action);
            actionStatus.applySteParams = new List<PA_StatusEffect.StatusEffectParams>();
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        s += ATK.GetInfo();
        s += exInfo;
        return s;
    }
}
