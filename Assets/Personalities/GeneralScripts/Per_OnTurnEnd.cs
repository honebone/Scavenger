using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnTurnEnd : Per_Master
{
    [SerializeField, Header("自身のターンで発動")] bool me;
    [SerializeField, Header("自身の召喚したキャラターンで発動")] bool myServant;
    [SerializeField, Header("自身以外の指定した条件のターンで発動")] bool otherChara;
    [SerializeField] CharactersManager.SearchCharaCondition otherCharaCond;

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (me && tep.myTurn)
        {
            Activate();
            return;
        }
        if (myServant && tep.turnChara.GetRootChara() == character)
        {
            Activate();
            return;
        }
        if (otherChara && charactersManager.ExamineCharacter(tep.turnChara, otherCharaCond, character))
        {
            Activate();
            return;
        }
    }
}
