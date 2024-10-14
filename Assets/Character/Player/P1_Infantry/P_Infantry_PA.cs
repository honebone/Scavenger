using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Infantry_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus move;
    [SerializeField]
    Action.ActionStatus shield;

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
            if(CD == 0)
            {
                available = true;
            }
        }
    }
    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        Character.CharacterStatus charaStatus = character.GetCharacterStatus();
        if (available && charaStatus.HP.GetPercent(charaStatus.maxHP) <= 20)
        {
            available = false;
            CD = CDRound;
            Action.ActionStatus action = shield;
            shield.actionOwner = character;
            Enqueue(action, true, new List<Character>() { character });
        }
    }
    public override void OnRoundEnd()
    {
        Character.CharacterStatus charaStatus = character.GetCharacterStatus();
        if (charaStatus.position.GetColumn()!=0)
        {
            Action.ActionStatus action = move;
            shield.actionOwner = character;
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
        string s = move.GetInfo(false, new Character.CharacterStatus()) +"\n";
        s += shield.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return (available) ? "シールド発動可能" : $"シールド再発動まで{CD}ラウンド";
    }
}
