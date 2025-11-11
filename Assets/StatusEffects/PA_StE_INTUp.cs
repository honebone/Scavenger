using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_INTUp : PA_StatusEffect
{
    [SerializeField] bool up;
    int n;
    public override void OnPAInit()
    {
        n = up ? 1 : -1;
        character.AddINT(0, StEStatus.value* n);
    }
    //public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    //{
    //    bool f = false;
    //    foreach (Action.OnAttackParams attackParams in onAttackParamsList)
    //    {
    //        if (attackParams.actionStatus.INTMod_max > 0)
    //        {
    //            f = true;
    //            break;
    //        }
    //    }
    //    if (f) AddStack(-1);
    //}

    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn && applyFlag) { AddStack(-1); }
    }

    public override void AtTheEnd()
    {
        character.AddINT(0, StEStatus.value * -1 * n);
    }
}
