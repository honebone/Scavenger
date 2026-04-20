using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_ActiveCond : Ability
{
    [SerializeField, Header("\n能力発動に必要な[条件を満たすキャラ]の必要人数")] int activeCond_req;
    [SerializeField, Header("能力発動に必要なキャラの条件")] CharactersManager.SearchCharaCondition activeCond;
    public override bool CheckSpecialCondition()
    {
        return charactersManager.SearchCharaWithCondition(activeCond, character).Count >= activeCond_req;
    }
}
