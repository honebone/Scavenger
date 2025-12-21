using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_OnTurnStart : Eq_Master
{
    [SerializeField] bool me;
    [SerializeField] bool myServant;
    [SerializeField] bool otherChara;
    [SerializeField] CharactersManager.SearchCharaCondition otherCharaCond;
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (me && myTurn)
        {
            Activate();
            return;
        }
        if (myServant && BattleManager.inst.GetCurrntTurnChara().GetRootChara() == character)
        {
            Activate();
            return;
        }
        if (otherChara && charactersManager.ExamineCharacter(BattleManager.inst.GetCurrntTurnChara().GetRootChara(), otherCharaCond, character))
        {
            Activate();
            return;
        }
    }
}
