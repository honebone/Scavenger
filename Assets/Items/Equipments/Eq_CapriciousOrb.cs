using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_CapriciousOrb : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus_forRef;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] Action.ActionStatus actionStatus2;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += equipmentStatus.GetInfo();
        s += actionStatus_forRef.GetInfo(false, new Character.CharacterStatus());
        return s;
    }

    public override void OnRoundEnd()
    {
        Action.ActionStatus action = actionStatus;
        Action.ActionStatus action2 = actionStatus2;
        List<Character> targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition));
        List<Character> heals = new List<Character>();
        List<Character> damages = new List<Character>();
        foreach(Character c in targets)
        {
            if (50.Dice()) { heals.Add(c); }
            else { damages.Add(c); }
        }

        Enqueue(action, true, heals, 0);
        Enqueue(action2, true, damages, 0);
    }
}
