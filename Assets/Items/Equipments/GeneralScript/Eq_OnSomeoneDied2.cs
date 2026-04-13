using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_OnSomeoneDied2 : Eq_Master
{
    public CharactersManager.SearchCharaCondition diedCondition;

    public override void OnSomeoneDied(Character died)
    {
        if(charactersManager.ExamineCharacter(died,diedCondition,character))Activate();
    }
}
