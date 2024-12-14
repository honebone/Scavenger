using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Werewolf_PA : PA_Personality
{
    [SerializeField] Action.ActionStatus applyWerewolf;
    [SerializeField] Action.ActionStatus removeWerewolf;
    [SerializeField] GameObject werewolf;

    bool act;
    public override void OnBattleStart()
    {
        act = false;
    }
    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        act = true;
    }
    public override void OnTurnEnd()
    {
        if (!act)
        {
            if (!character.CheckHasStE(werewolf)) { Enqueue_Self(applyWerewolf); }
            else { Enqueue_Self(removeWerewolf); }
        }
        act = false;
    }

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += applyWerewolf.GetInfo(false, new Character.CharacterStatus());
        s += "\n"+removeWerewolf.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
