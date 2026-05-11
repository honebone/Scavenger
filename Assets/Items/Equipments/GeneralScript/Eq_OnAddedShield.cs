using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_OnAddedShield : Eq_Master
{
    [SerializeField] bool toAdder;
    public override void OnAddedShield(int value, Action.ActionParams actionParams)
    {
        if (toAdder) Activate(new List<Character> { actionParams.owner });
        else Activate();
    }
}
