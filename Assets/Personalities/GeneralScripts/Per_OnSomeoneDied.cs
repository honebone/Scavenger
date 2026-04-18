using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnSomeoneDied : Per_Master
{
    public CharactersManager.SearchCharaCondition diedCondition;

    public override void OnSomeoneDied(Character died)
    {
        if (charactersManager.ExamineCharacter(died, diedCondition, character)) Activate();
    }
}
