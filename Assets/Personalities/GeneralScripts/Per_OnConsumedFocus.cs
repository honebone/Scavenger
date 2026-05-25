using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Per_OnConsumedFocus : Per_Master
{
    [SerializeField] bool toFocused;
    [SerializeField] bool forEachConsumed;
    public override void OnFocus(List<Action.OnFocusParams> focusParamsList)
    {
        List<Character> targetList = new List<Character>();

        if (!forEachConsumed)
        {
            if (toFocused) Activate(new List<Character>(focusParamsList.Select(f => f.actionParams.target)));
            else Activate();
            return;
        }

        foreach (Action.OnFocusParams focusParams in focusParamsList)
        {
            if (toFocused) Activate(new List<Character> { focusParams.actionParams.target });
            else Activate();

        }
    }
}
