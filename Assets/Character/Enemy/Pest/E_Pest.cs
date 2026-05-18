using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Pest : PA_Personality
{
    [SerializeField] Action.ActionStatus heal;

    public override void OnDie(Character killer)
    {
        if (killer != null)
        {
            Enqueue(heal, true, new List<Character> { killer });
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += heal.GetInfo(false, new Character.CharacterStatus()) + "\n";
        return s;
    }
}
