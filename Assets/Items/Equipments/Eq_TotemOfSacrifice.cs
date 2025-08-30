using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_TotemOfSacrifice : PA_Equipment
{
    public float DMGPercent;
    public Action.ActionStatus kill;
    public Action.ActionStatus actionStatus;
    public CharactersManager.SearchCharaCondition condition;

    public override void OnSomeoneSummoned(Character summoner, List<Action.OnSummonParams> onSummonParamsList)
    {
        foreach (var summoned in onSummonParamsList)
        {
            if (summoned.summoned != null && summoned.summoned.PlayerPos())
            {
                Enqueue(kill, true, new List<Character> { summoned.summoned });
            }
        }
    }

    public override void OnSomeoneDied(Character died)
    {
        Character.CharacterStatus status = died.CharaStatus();
        if (died.PlayerPos() && !status.characterTags.Contains(CharacterData.CharacterTag.obstacle))
        {
            Action.ActionStatus action = actionStatus;
            int DMG = ((status.ATK + status.INT) * DMGPercent / 100f).ToInt();
            action.exINTDMG_int = DMG;

            Enqueue_SearchTarget(action, condition, 1);
        }
    }
}
