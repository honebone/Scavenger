using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StEU_Sniper_Bullet : PA_StatusEffect
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    public override void OnSomeoneDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        if (onDamageParamsList == null || onDamageParamsList.Count == 0) return;
        if (!charactersManager.ExamineCharacter(onDamageParamsList[0].ap.owner, condition, character)) return;
        if (!onDamageParamsList[0].ap.actionStatus.abilityEffect) return;

        List<Character> pool = onDamageParamsList.Select(p => p.ap.target).Where(c => c.CheckAlive() && !c.IsObstacle()).ToList();
        if (pool.Count == 0) return;

        Enqueue(actionStatus, true, pool, 1);
        AddStack(-1);
    }

    public override string GetAdditionalInfo()
    {
        return actionStatus.GetInfo();
    }
}
