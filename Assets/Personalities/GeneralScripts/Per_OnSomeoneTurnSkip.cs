using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnSomeoneTurnSkip : Per_Master
{
    [SerializeField] bool toSkipped;
    [SerializeField] CharactersManager.SearchCharaCondition skipCharaCond;
    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (!tep.actThisTurn && charactersManager.ExamineCharacter(tep.turnChara, skipCharaCond, character))
        {
            if (toSkipped) Activate(new List<Character>() { tep.turnChara });
            else Activate();
        }
    }
}
