using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Werewolf_PA : PA_Personality
{
    [SerializeField] Action.ActionStatus applyWerewolf;
    [SerializeField] Action.ActionStatus removeWerewolf;
    [SerializeField] GameObject werewolf;

    [SerializeField] GameObject sprite_human;

    bool act;
    public override void OnBattleStart()
    {
        act = false;
    }
    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        act = true;
    }
    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (!act&&myTurn)
        {
            if (!character.CheckHasStE(werewolf)) { Enqueue_Self(applyWerewolf); }
            else { Enqueue_Self(removeWerewolf); }
        }
        act = false;
    }

    public override void OnBattleEnd()
    {
       character.SetCharaSprite(sprite_human);
    }

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += applyWerewolf.GetInfo(false, new Character.CharacterStatus());
        s += "\n"+removeWerewolf.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
