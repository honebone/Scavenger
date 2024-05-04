using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Per_General : PA_Personality
{
   [SerializeField]
    Character.CharaStatusMod statusMod;
    [SerializeField]
    public List<GameObject> actionMods;

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += statusMod.GetInfo();
        foreach (GameObject actionMod in actionMods)
        {
            s += actionMod.GetComponent<ActionMod>().GetActionModStatus().GetModInfo();
        }
        return s;
    }

    public override void OnPAInit()
    {
        character.ModifyStatus(statusMod, true);
        foreach (GameObject mod in actionMods) { character.AddActionMod(mod, true); }
    }
}
