using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnSummon : Per_Master
{
    [SerializeField] bool forEachSummon;
    public override void OnSummon(List<Action.OnSummonParams> onSummonParamsList)
    {
        if (forEachSummon)
        {
            for (int i = 0; i < onSummonParamsList.Count; i++) { Activate(); }
        }
        else { Activate(); }
    }
}
