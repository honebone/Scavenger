using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnTurnStart : Per_Master
{
    [SerializeField,Header("自身のターンで発動")] bool me;
    [SerializeField, Header("自身の召喚したキャラターンで発動")] bool myServant;
    [SerializeField, Header("自身以外の指定した条件のターンで発動")] bool otherChara;
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
        if (otherChara && charactersManager.ExamineCharacter(BattleManager.inst.GetCurrntTurnChara(), otherCharaCond, character))
        {
            Activate();
            return;
        }
    }
}
