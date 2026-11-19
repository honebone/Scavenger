using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_OnHeal : Eq_Master
{
    [SerializeField] bool onlyAbility;
    [SerializeField] bool onlyPlayer;
    [SerializeField] bool forEachHeal;
    [SerializeField] bool onlyOnHealOther;
    public override void OnHeal(List<Action.OnHealParams> onHealParamsList)
    {
        if (!onHealParamsList[0].ability && onlyAbility) return;
        for (int i = 0; i < onHealParamsList.Count; i++)
        {
            if (onlyOnHealOther && onHealParamsList[i].target == character) continue;
            if (onlyPlayer && !onHealParamsList[i].target.IsPlayer()) continue;

            Activate();
            if (!forEachHeal) break;
        }
    }
}
