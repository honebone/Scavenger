using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_NightCloak : PA_Equipment
{
    [SerializeField] int threshold;
    [SerializeField] Action.ActionStatus actionStatus;

    [SerializeField] int CDRound;
    int CD;
    bool available;
    public override void OnBattleStart()
    {
        available = true;
        CD = 0;
    }
    public override void OnRoundStart()
    {
        if (CD > 0)
        {
            CD--;
            if (CD == 0)
            {
                available = true;
                Log("再発動可能");
            }
        }
    }
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        Character.CharacterStatus charaStatus = character.CharaStatus();
        if (available && charaStatus.HP.GetPercent(charaStatus.maxHP) <= threshold)
        {
            available = false;
            CD = CDRound;
            Action.ActionStatus action = actionStatus;
            actionStatus.actionOwner = character;
            Enqueue(action, true, new List<Character>() { character });
        }
    }

    public override void OnBattleEnd()
    {
        available = true;
        CD = 0;
    }
    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return (available) ? "発動可能" : $"再発動まで{CD}ラウンド";
    }
}
