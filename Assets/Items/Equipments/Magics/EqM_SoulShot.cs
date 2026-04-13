using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_SoulShot : Eq_Magic
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnSomeoneDied(Character died)
    {
        if (!died.CharaStatus().characterTags.Contains(CharacterData.CharacterTag.obstacle))
        {
            Cast();
        }
    }

    public override void Cast()
    {
        List<Character> targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition));

        if (Enqueue(actionStatus, true, targets, 1))
        {
            character.OnCast(this);
        }

    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
}
