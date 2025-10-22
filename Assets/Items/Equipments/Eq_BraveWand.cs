using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eq_BraveWand : PA_Equipment
{
    public int INT;
    public int maxCount;

    int count;
    public override void OnApplyStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        onApplyStEParamsList.ForEach(x =>
        {
            if (count < maxCount && x.appliedParams.Any(y => y.GetStatusEffectStatus().StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff))
            {
                character.AddINT(0, INT);
                count++;
                Log($"{"INT".ToSpr_withName(null, true)}+{INT}Åì(+{INT * count}Åì)");
            }
        });
    }
    public override void OnBattleEnd()
    {
        character.AddINT(0, -INT * count);
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"{"INT".ToSpr_withName()}+{INT * count}Åì";
    }
}
