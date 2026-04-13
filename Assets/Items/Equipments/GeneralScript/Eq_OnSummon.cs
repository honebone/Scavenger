using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eq_OnSummon : Eq_Master
{
    [SerializeField] bool forEachSummon;
    [SerializeField] bool toSummon;
    public override void OnSummon(List<Action.OnSummonParams> onSummonParamsList)
    {
        if (forEachSummon)
        {
            for (int i = 0; i < onSummonParamsList.Count; i++)
            {
                if (toSummon) Activate(new List<Character> { onSummonParamsList[i].summoned });
                else Activate();
            }
        }
        else
        {
            if (toSummon)
            {
                List<Character> targets = onSummonParamsList.Select(p => p.summoned).ToList();
                Activate(targets);
            }
            else Activate();
        }
    }
}
