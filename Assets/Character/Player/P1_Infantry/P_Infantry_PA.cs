using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Infantry_PA : PA_Personality
{
    [SerializeField]
    Action.ActionStatus move;
    [SerializeField]
    Action.ActionStatus shield;

    bool shieldActivated;
    public override void OnBattleStart()
    {
        shieldActivated = false;
    }

    public override void OnDamaged(int DMG, Character attacker)
    {
        Character.CharacterStatus charaStatus = character.GetCharacterStatus();
        if (!shieldActivated&&charaStatus.HP.GetPercent(charaStatus.maxHP)<=20)
        {
            shieldActivated = true;
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
    public override string GetPAInfo_Base()
    {
        string s = move.GetInfo(true, character.GetCharacterStatus())+"\n";
        s += shield.GetInfo(true, character.GetCharacterStatus());
        return s;
    }
}
